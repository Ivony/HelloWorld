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
  public abstract class GameRules : GameRulesBase
  {


    private GameRuleDataItemCollection _collection;



    private IJsonDataResolver _dataResolver;
    private ITypeResolver _typeResolver;
    private IGameDataService _dataService;

    public IGameDataService DataService { get { return GameHost.DataService; } }

    protected GameRules( IJsonDataResolver dataResolver = null, ITypeResolver typeResolver = null )
    {

      _dataResolver = dataResolver ?? new JsonDataResolver();
      _typeResolver = typeResolver ?? new TypeResolver();
    }


    /// <summary>
    /// 初始化游戏规则
    /// </summary>
    public override void Initialize()
    {

      _collection = new GameRuleDataItemCollection();


      foreach ( var dataItem in LoadRulesData() )
      {

        var item = LoadData( dataItem );
        if ( item == null )
          continue;

        _collection.Add( item );
      }
    }




    /// <summary>
    /// 获取指定类型的规则集合
    /// </summary>
    /// <typeparam name="T">规则类型</typeparam>
    /// <returns>指定类型的规则集合</returns>
    public T[] GetRules<T>() where T : GameRuleItem
    {
      return _collection.OfType<T>().ToArray();
    }



    protected ActionDescriptor[] AllActions()
    {
      return _collection.OfType<ActionDescriptor>().ToArray();
    }




    /// <summary>
    /// 加载所有的游戏规则数据
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerable<JObject> LoadRulesData()
    {
      return _dataResolver.LoadAllData();
    }




    /// <summary>
    /// 加载游戏规则数据并创建相应描述符对象
    /// </summary>
    /// <param name="data">游戏规则数据</param>
    /// <returns></returns>
    protected virtual GameRuleItem LoadData( JObject data )
    {
      var type = GetType( data.Value<String>( "Type" ) );

      if ( type == null )
        return null;

      if ( typeof( GameRuleItem ).IsAssignableFrom( type ) == false )
        return null;


      var instance = Activator.CreateInstance( type ) as GameRuleItem;
      instance.InitializeData( this, data );

      return instance;
    }


    /// <summary>
    /// 根据名称获取类型
    /// </summary>
    /// <param name="typeName">类型名称</param>
    /// <returns>类型实例</returns>
    public override Type GetType( string typeName )
    {
      return _typeResolver.GetType( typeName );
    }





    public override GameRuleItem GetDataItem( Guid id )
    {

      if ( _collection == null )
        throw new InvalidOperationException( "游戏规则尚未初始化" );


      if ( _collection.Contains( id ) == false )
        throw new InvalidDataException( "未注册的游戏数据 ID: " + id );


      return _collection[id];
    }







    /// <summary>
    /// 获取玩家初始点
    /// </summary>
    /// <param name="dataService">数据服务</param>
    /// <returns></returns>
    public override Coordinate GetInitiation()
    {

      var i = 0;
      while ( i < 100 )
      {

        i++;
        var initiation = Coordinate.RandomCoordinate( 1000, 1000 );

        if ( initiation.NearlyCoordinates( 6 ).Any( item => DataService.GetPlace( item ).Owner != null ) )
          continue;

        return initiation;
      }

      throw new InvalidOperationException( "无法找到合适的初始点" );
    }




    /// <summary>
    /// 初始化玩家对象
    /// </summary>
    /// <param name="player"></param>
    public override void InitializePlayer( GamePlayer player )
    {
    }



    public override Place CreatePlace( Coordinate coordinate )
    {
      return new Place( coordinate );
    }

  }
}
