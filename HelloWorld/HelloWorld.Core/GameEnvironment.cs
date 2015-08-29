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


    private class GameDataItemCollection : KeyedCollection<Guid, GameDataItem>
    {
      protected override Guid GetKeyForItem( GameDataItem item )
      {
        return item.Guid;
      }

    }



    private static GameDataItemCollection _collection;

    private static Dictionary<BuildingDescriptor, HashSet<ActionDescriptor>> _actionMap = new Dictionary<BuildingDescriptor, HashSet<ActionDescriptor>>();


    /// <summary>
    /// 初始化游戏环境
    /// </summary>
    /// <param name="typeResolver"></param>
    public static void Initialize( ITypeResolver typeResolver = null )
    {
      Initialize( ConfigurationManager.AppSettings["GamePath"], typeResolver );
    }

    /// <summary>
    /// 初始化游戏环境
    /// </summary>
    /// <param name="path"></param>
    /// <param name="typeResolver"></param>
    public static void Initialize( string path, ITypeResolver typeResolver )
    {

      typeResolver = typeResolver ?? new TypeResolver();


      if ( path == null || Directory.Exists( path ) == false )
        throw new InvalidOperationException( "无法找到游戏规则配置" );

      _collection = new GameDataItemCollection();




      var fileList = Directory.GetFiles( path )
        .Concat( Directory.GetFiles( Path.Combine( path, "items" ) ) )
        .Concat( Directory.GetFiles( Path.Combine( path, "buildings" ) ) )
        .Concat( Directory.GetFiles( Path.Combine( path, "productions" ) ) )
        .Concat( Directory.GetFiles( Path.Combine( path, "constructions" ) ) );



      foreach ( var file in fileList )
        LoadData( file, typeResolver );

    }



    private static void LoadData( string filepath, ITypeResolver typeResolver )
    {
      var data = JObject.Parse( File.ReadAllText( filepath ) );

      var id = data.GuidValue( "ID" );
      var type = typeResolver.GetType( data.Value<String>( "Type" ) );

      if ( type == null )
        return;

      if ( typeof( GameDataItem ).IsAssignableFrom( type ) == false )
        return;

      var method = type.GetMethod( "FromData", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy );
      if ( method == null )
        return;

      var instance = (GameDataItem) method.Invoke( null, new object[] { id, data } );
      _collection.Add( instance );


      var production = instance as ActionDescriptor;
      if ( production != null )
      {
        if ( _actionMap.ContainsKey( production.Building ) == false )
          _actionMap.Add( production.Building, new HashSet<ActionDescriptor>() { production } );

        else
          _actionMap[production.Building].Add( production );
        return;
      }
    }





    public static GameDataItem GetDataItem( Guid id )
    {

      if ( _collection == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      if ( _collection.Contains( id ) == false )
        throw new InvalidDataException( "未注册的游戏数据 ID: " + id );


      return _collection[id];
    }



    public static T GetDataItem<T>( Guid id ) where T : GameDataItem
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
    public static ActionDescriptor[] GetActions( BuildingDescriptor building )
    {
      if ( _actionMap == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      HashSet<ActionDescriptor> result;
      if ( _actionMap.TryGetValue( building, out result ) )
        return result.ToArray();

      else
        return new ActionDescriptor[0];
    }

  }
}
