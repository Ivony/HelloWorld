using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
    private static GameItemDataCollection<ProductionDescription> _productions;
    private static GameItemDataCollection<ConstructionDescriptor> _constructions;




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
      _productions = new GameItemDataCollection<ProductionDescription>();
      _constructions = new GameItemDataCollection<ConstructionDescriptor>();


      foreach ( var file in Directory.GetFiles( path, "*", SearchOption.AllDirectories ) )
      {
        var data = JObject.Parse( File.ReadAllText( file ) );

        var id = data.GuidValue( "ID" );
        var type = data.Value<String>( "Type" );


        switch ( type )
        {
          case "Item":
            _items.Add( ItemDescriptor.FromData( id, data ) );
            break;

          case "Building":
            _buildings.Add( BuildingDescriptor.FromData( id, data ) );
            break;

          case "Construction":
            _constructions.Add( ConstructionDescriptor.FromData( id, data ) );
            break;

          case "Production":
            _productions.Add( ProductionDescription.FromData( id, data ) );
            break;

        }
      }
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
    public static ProductionDescription GetProduction( Guid id )
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
  }
}
