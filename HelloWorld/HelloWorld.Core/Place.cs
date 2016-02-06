using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  /// <summary>
  /// 代表一个地块
  /// </summary>
  public class Place : GameDataItem
  {

    public Place( IGameDataService service, Coordinate coordinate ) : base( service )
    {
      Coordinate = coordinate;
    }




    protected override void Initialize()
    {
      base.Initialize();

      {
        var items = ItemListJsonConverter.FromJson( (JObject) DataObject.Resources );
        Resources = new ItemCollection( items, collection =>
        {
          DataObject.Resources = ItemListJsonConverter.ToJson( collection );
        } );
      }


      Acting = PlaceActing.FromData( this, (JObject) DataObject.Acting );

    }



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
    /// 地块坐标
    /// </summary>
    public Coordinate Coordinate { get; private set; }

    /// <summary>
    /// 地块上的建筑
    /// </summary>
    public BuildingDescriptor Building
    {
      get { return GameHost.GameRules.GetDataItem<BuildingDescriptor>( JsonObject.GuidValue( "Building" ) ); }
      set { DataObject.Building = value.Guid; }
    }


    /// <summary>
    /// 确保指定的单位在自己的单位列表中
    /// </summary>
    /// <param name="unit"></param>
    internal void EnsureUnit( Unit unit )
    {
      throw new NotImplementedException();
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
    /// 正在进行的活动
    /// </summary>
    public PlaceActing Acting { get; private set; }


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
    public void Check()
    {
      lock ( SyncRoot )
      {
        Collect();

        if ( Acting != null )
          Acting.Check();

        Building.Check( this );

        SaveActing();
      }
    }


    /// <summary>
    /// 收集地块上所有的资源
    /// </summary>
    /// <param name="player"></param>
    public void Collect()
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
    public object GetInfo( GamePlayer player )
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
    /// 获取可以执行的活动列表
    /// </summary>
    /// <returns></returns>
    public virtual ActionDescriptor[] GetActions()
    {

      return GameHost.GameRules.GetActions( this );
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
