using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  /// <summary>
  /// 代表一个地块的抽象
  /// </summary>
  public class Place : PlaceBase
  {


    /// <summary>
    /// 创建 Place 对象
    /// </summary>
    /// <param name="coordinate">地块所处坐标</param>
    public Place( Coordinate coordinate ) : base( coordinate ) { }



    /// <summary>
    /// 初始化 Place 对象
    /// </summary>
    protected override void Initialize()
    {
      {
        var items = ItemListJsonConverter.FromJson( (JObject) DataObject.Resources );
        Resources = new ItemCollection( items, collection =>
        {
          DataObject.Resources = ItemListJsonConverter.ToJson( collection );
        } );
      }


      Acting = PlaceActing.FromData( this, (JObject) DataObject.Acting );


      Terrain = InitializeImmovableInstance<Terrain>( (JsonDataObject) DataObject.Terrain );
      TraficNetwork = InitializeImmovableInstance<TraficNetwork>( (JsonDataObject) DataObject.TraficNetwork );
      Building = InitializeImmovableInstance<Building>( (JsonDataObject) DataObject.Building );



      base.Initialize();
    }



    protected T InitializeImmovableInstance<T>( JsonDataObject dataObject ) where T : Immovable
    {
      if ( dataObject == null )
        return null;

      var instance = GameHost.GameRules.CreateInstance<T>( (string) dataObject["Type"] );
      instance.InitializeData( this, dataObject );

      return instance;
    }



    /// <summary>
    /// 正在进行的活动
    /// </summary>
    public PlaceActing Acting { get; private set; }


    internal void SetActing( PlaceActing acting )
    {
      if ( Acting != null && acting != null )
        throw new InvalidOperationException();

      Acting = acting;
      SaveActing();
    }


    private void SaveActing()
    {
      if ( Acting == null )
        DataObject.Acting = null;

      else
        DataObject.Acting = Acting.ToJson();
    }



    /// <summary>
    /// 地块上的地形
    /// </summary>
    public Terrain Terrain { get; private set; }

    /// <summary>
    /// 设置地形
    /// </summary>
    /// <param name="building">建筑描述</param>
    public virtual void SetTerrain( Guid? id )
    {

      if ( id == null )
      {
        DataObject.Terrain = null;
        DataObject.TraficNetwork = null;
        DataObject.Building = null;

        return;
      }

      var descriptor = GameHost.GameRules.GetDataItem<TerrainDescriptor>( id );


      DataObject.Terrain = new JObject();
      DataObject.Terrain.Descriptor = descriptor.Guid;

      DataObject.TraficNetwork = null;
      DataObject.Building = null;


      var type = descriptor.InstanceType;
      var instance = (Terrain) Activator.CreateInstance( type );

      instance.InitializeData( this, DataObject.Terrain );


      Terrain = instance;


    }



    /// <summary>
    /// 地块上的交通网络
    /// </summary>
    public TraficNetwork TraficNetwork { get; private set; }



    /// <summary>
    /// <summary>
    /// 地块上的建筑
    /// </summary>
    public Building Building { get; private set; }



    /// <summary>
    /// 设置地块上的建筑
    /// </summary>
    /// <param name="building">建筑描述</param>
    public virtual void SetBuilding( Guid? id )
    {
      var descriptor = GameHost.GameRules.GetDataItem<BuildingDescriptor>( id );


      DataObject.Building = new JObject();
      DataObject.Building.Descriptor = descriptor.Guid;


      var type = descriptor.InstanceType;
      var instance = (Building) Activator.CreateInstance( type );

      instance.InitializeData( this, DataObject.Building );


      Building = instance;

    }



    /// <summary>
    /// 获取当前地块上所有单位
    /// </summary>
    /// <returns></returns>
    public Unit[] GetUnits()
    {
      return DataService.GetUnits( Coordinate );
    }




    /// <summary>
    /// 获取地块上存在的资源
    /// </summary>
    public ItemCollection Resources
    {
      get;
      private set;
    }


    /// <summary>
    /// 地块拥有者的玩家 ID
    /// </summary>
    public Guid? Owner
    {
      get { return DataObject.Owner; }
      set { DataObject.Owner = value; }
    }


    /// <summary>
    /// 获取地块拥有者的玩家对象
    /// </summary>
    /// <returns></returns>
    public GamePlayer GetPlayer()
    {

      if ( Owner == null )
        return null;

      return DataService.GetPlayer( Owner.Value );
    }



    /// <summary>
    /// 上次检查时间
    /// </summary>
    public DateTime CheckPoint
    {
      get { return DataObject.CheckPoint; }
      set { DataObject.CheckPoint = value; }
    }



    /// <summary>
    /// 对地块执行例行检查，处理例行事项
    /// </summary>
    public override void Check( DateTime now )
    {
      lock ( SyncRoot )
      {
        Collect();

        if ( Acting != null )
          Acting.Check( now );

        if ( Terrain != null )
          Terrain.Check( now );

        if ( TraficNetwork != null )
          TraficNetwork.Check( now );

        if ( Building != null )
          Building.Check( now );


        foreach ( var item in GetUnits() )
          item.Check( now );


        SaveActing();
      }
    }


    /// <summary>
    /// 收集地块上所有的资源
    /// </summary>
    public virtual void Collect()
    {
      lock ( SyncRoot )
      {
        var player = GetPlayer();
        if ( player != null )
          player.Resources.Collect( Resources );
      }
    }


    /// <summary>
    /// 获取用于展示的地块信息
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual object GetInfo( GamePlayer player )
    {
      var relativeCoordinate = Coordinate.ToRelative( player );

      var data = JObject.FromObject(
      new
      {
        Coordinate = relativeCoordinate,
        Terrain = Terrain == null ? null : Terrain.GetInfo(),
        TraficNetwork = TraficNetwork == null ? null : TraficNetwork.GetInfo(),
        Building = Building == null ? null : Building.GetInfo(),
        Resources,
        IsMine = Owner == player.Guid,

        Nearly = relativeCoordinate.NearlyCoordinates(),
      } );

      if ( Acting == null )
        data["Actions"] = JArray.FromObject( GetActions( player ).Select( item => item.GetInfo() ) );

      else
        data["Action"] = JObject.FromObject( Acting.GetInfo() );

      return data;
    }



    /// <summary>
    /// 重写 Equals 方法比较两个地块对象。
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object obj )
    {
      var place = obj as Place;

      if ( place == null )
        return false;


      return place.Coordinate == this.Coordinate;
    }


    /// <summary>
    /// 重写 GetHashCode 方法，用坐标作为地块标识
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
      return Coordinate.GetHashCode();
    }


    /// <summary>
    /// 重写 ToString 方法，输出地块坐标
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return Coordinate.ToString();
    }





    private static object _sync = new object();
    private static ActionDescriptor[] _actionsCache;

    /// <summary>
    /// 获取该地块可以执行的所有操作列表
    /// </summary>
    /// <param name="player">当前玩家对象</param>
    /// <returns></returns>
    public override ActionDescriptor[] GetActions( GamePlayer player )
    {

      lock ( _sync )
      {
        if ( _actionsCache == null )
          _actionsCache = ( (GameRules) GameHost.GameRules ).GetRules<ActionDescriptor>();


        return _actionsCache.Where( item => item.CanStartAt( player, this ) ).ToArray();
      }
    }
  }
}
