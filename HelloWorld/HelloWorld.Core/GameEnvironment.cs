using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace HelloWorld
{


  /// <summary>
  /// 游戏全局规则和对象管理器
  /// </summary>
  public static class GameEnvironment
  {


    private class GameItemDataCollection<T> : KeyedCollection<Guid, T> where T : GameItemData
    {
      protected override Guid GetKeyForItem( T item )
      {
        return item.Guid;
      }

    }


    private static GameItemDataCollection<ItemDescriptor> _items;
    private static GameItemDataCollection<BuildingDescriptor> _buildings;
    private static GameItemDataCollection<ProductionDescriptor> _productions;
    private static GameItemDataCollection<ConstructionDescriptor> _constructions;


    private static Dictionary<BuildingDescriptor, HashSet<ConstructionDescriptor>> _constructionsMap = new Dictionary<BuildingDescriptor, HashSet<ConstructionDescriptor>>();
    private static Dictionary<BuildingDescriptor, HashSet<ProductionDescriptor>> _productionsMap = new Dictionary<BuildingDescriptor, HashSet<ProductionDescriptor>>();


    public static void Initialize()
    {
      Initialize( ConfigurationManager.AppSettings["GamePath"] );
    }

    public static void Initialize( string path )
    {

      if ( path == null || Directory.Exists( path ) == false )
        throw new InvalidOperationException( "无法找到游戏规则配置" );


      _items = new GameItemDataCollection<ItemDescriptor>();
      _buildings = new GameItemDataCollection<BuildingDescriptor>();
      _productions = new GameItemDataCollection<ProductionDescriptor>();
      _constructions = new GameItemDataCollection<ConstructionDescriptor>();



      var fileList = Directory.GetFiles( path )
        .Concat( Directory.GetFiles( Path.Combine( path, "items" ) ) )
        .Concat( Directory.GetFiles( Path.Combine( path, "buildings" ) ) )
        .Concat( Directory.GetFiles( Path.Combine( path, "productions" ) ) )
        .Concat( Directory.GetFiles( Path.Combine( path, "constructions" ) ) );



      foreach ( var file in fileList )
        LoadData( file );

    }



    private static void LoadData( string filepath )
    {
      var data = JObject.Parse( File.ReadAllText( filepath ) );

      var id = data.GuidValue( "ID" );
      var type = GetType( data.Value<String>( "Type" ) );


      var instance = type.GetMethod( "FromData", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy ).Invoke( null, new object[] { id, data } );



      var item = instance as ItemDescriptor;
      if ( item != null )
      {
        _items.Add( item );
        return;
      }


      var building = instance as BuildingDescriptor;
      if ( building != null )
      {
        _buildings.Add( building );
        return;
      }


      var construction = instance as ConstructionDescriptor;
      if ( construction != null )
      {
        _constructions.Add( construction );

        if ( _constructionsMap.ContainsKey( construction.RawBuilding ) == false )
          _constructionsMap.Add( construction.RawBuilding, new HashSet<ConstructionDescriptor>() { construction } );

        else
          _constructionsMap[construction.RawBuilding].Add( construction );
        return;
      }


      var production = instance as ProductionDescriptor;
      if ( production != null )
      {
        _productions.Add( production );

        if ( _constructionsMap.ContainsKey( production.Building ) == false )
          _productionsMap.Add( production.Building, new HashSet<ProductionDescriptor>() { production } );

        else
          _productionsMap[production.Building].Add( production );
        return;
      }
    }

    private static Type GetType( string type )
    {

      switch ( type )
      {
        case "Item":
          return typeof( ItemDescriptor );

        case "Building":
          return typeof( BuildingDescriptor );

        case "Construction":
          return typeof( ConstructionDescriptor );

        case "Production":
          return typeof( ProductionDescriptor );
      }


      throw new NotImplementedException();
    }




    /// <summary>
    /// 获取一个物品描述
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ItemDescriptor GetItem( Guid id )
    {
      if ( _items == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      if ( _items.Contains( id ) == false )
        throw new InvalidDataException( "未注册的物品 ID: " + id );

      return _items[id];
    }


    /// <summary>
    /// 获取一个建筑描述
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static BuildingDescriptor GetBuilding( Guid id )
    {
      if ( _buildings == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      if ( _buildings.Contains( id ) == false )
        throw new InvalidDataException( "未注册的建筑 ID: " + id );

      return _buildings[id];
    }


    /// <summary>
    /// 获取一条生产规则
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ProductionDescriptor GetProduction( Guid id )
    {
      if ( _productions == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      if ( _productions.Contains( id ) == false )
        throw new InvalidDataException( "未注册的生产规则 ID: " + id );

      return _productions[id];
    }

    /// <summary>
    /// 获取一条建造规则
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ConstructionDescriptor GetConstruction( Guid id )
    {
      if ( _constructions == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      if ( _constructions.Contains( id ) == false )
        throw new InvalidDataException( "未注册的建造规则 ID: " + id );

      return _constructions[id];
    }



    /// <summary>
    /// 获取某个建筑可以进行的生产
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public static ProductionDescriptor[] GetProductions( BuildingDescriptor building )
    {
      if ( _productionsMap == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      HashSet<ProductionDescriptor> result;
      if ( _productionsMap.TryGetValue( building, out result ) )
        return result.ToArray();

      else
        return new ProductionDescriptor[0];
    }


    /// <summary>
    /// 获取某个建筑可以进行的升级或者改造
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public static ConstructionDescriptor[] GetConstructions( BuildingDescriptor building )
    {
      if ( _constructionsMap == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      HashSet<ConstructionDescriptor> result;
      if ( _constructionsMap.TryGetValue( building, out result ) )
        return result.ToArray();

      else
        return new ConstructionDescriptor[0];
    }

  }
}
