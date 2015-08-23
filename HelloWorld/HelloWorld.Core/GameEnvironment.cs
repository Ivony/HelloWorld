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


    private class GameItemDataCollection : KeyedCollection<Guid, GameItemData>
    {
      protected override Guid GetKeyForItem( GameItemData item )
      {
        return item.Guid;
      }

    }



    private static GameItemDataCollection _collection;

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

      var method = type.GetMethod( "FromData", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy );
      if ( method == null )
        return;

      var instance = (GameItemData) method.Invoke( null, new object[] { id, data } );
      _collection.Add( instance );


      var construction = instance as ConstructionDescriptor;
      if ( construction != null )
      {
        _collection.Add( construction );

        if ( _constructionsMap.ContainsKey( construction.RawBuilding ) == false )
          _constructionsMap.Add( construction.RawBuilding, new HashSet<ConstructionDescriptor>() { construction } );

        else
          _constructionsMap[construction.RawBuilding].Add( construction );
        return;
      }


      var production = instance as ProductionDescriptor;
      if ( production != null )
      {
        _collection.Add( production );

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






    public static GameItemData GetDataItem( Guid id )
    {

      if ( _collection == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      if ( _collection.Contains( id ) == false )
        throw new InvalidDataException( "未注册的游戏数据 ID: " + id );


      return _collection[id];
    }



    public static T GetDataItem<T>( Guid id ) where T : GameItemData
    {

      var result = GetDataItem( id ) as T;
      if ( result == null )
        throw new InvalidDataException( "游戏数据注册类型错误 ID: " + id );

      return result;
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
