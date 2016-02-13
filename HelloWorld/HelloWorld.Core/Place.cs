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
  public abstract class Place : PlaceBase
  {


    /// <summary>
    /// 创建 Place 对象
    /// </summary>
    /// <param name="coordinate">地块所处坐标</param>
    protected Place( Coordinate coordinate ) : base( coordinate ) { }



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


      var buildingData = (JObject) DataObject.Building;
      if ( buildingData != null )
      {
        var building = GameHost.GameRules.CreateInstance<Building>( (string) buildingData["Type"] );
        building.InitializeData( this, DataObject.Building );

        Building = building;
      }

      else
        Building = null;

      base.Initialize();
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
    /// <summary>
    /// 地块上的建筑
    /// </summary>
    public Building Building
    {
      get;
      private set;
    }



    /// <summary>
    /// 设置地块上的建筑
    /// </summary>
    /// <param name="building">建筑描述</param>
    public virtual void SetBuilding( BuildingDescriptor building )
    {

      DataObject.Building = new JObject();
      DataObject.Building.Descriptor = building.Guid;




      var type = building.BuildingType;
      var instance = (Building) Activator.CreateInstance( type );

      instance.InitializeData( this, DataObject.Building );


      Building = instance;

    }





    /// <summary>
    /// 确保指定的单位在自己的单位列表中
    /// </summary>
    /// <param name="unit"></param>
    internal void EnsureUnit( Unit unit )
    {
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


        Building.Check( now );


        foreach ( var item in DataService.GetUnits( this.Coordinate ) )
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

      var coordinate = Coordinate - player.Initiation;


      var data = JObject.FromObject(
      new
      {
        Coordinate = coordinate,
        Building = Building.GetInfo(),
        Resources,
        IsMine = Owner == player.UserID,

        Nearly = coordinate.NearlyCoordinates(),
      } );

      if ( Acting == null )
        data["Actions"] = JArray.FromObject( GetActions().Select( item => item.GetInfo() ) );

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






    /// <summary>
    /// 获取相对于地块所在玩家而言地块的坐标
    /// </summary>
    /// <returns></returns>
    public Coordinate GetPlayerCoordinate()
    {

      if ( Owner == null )
        throw new InvalidOperationException();

      return GetPlayerCoordinate( GetPlayer() );
    }


    /// <summary>
    /// 获取相对于指定玩家而言地块的坐标
    /// </summary>
    /// <param name="player">玩家对象</param>
    /// <returns></returns>
    public Coordinate GetPlayerCoordinate( GamePlayer player )
    {

      if ( player == null )
        throw new ArgumentNullException( "player" );

      return Coordinate - player.Initiation;
    }
  }
}
