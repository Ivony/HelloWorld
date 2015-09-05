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
  public abstract class Place
  {

    protected Place( GameDataService service, Coordinate coordinate )
    {
      DataService = service;
      Coordinate = coordinate;
      SyncRoot = new object();
    }




    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    public object SyncRoot { get; private set; }


    /// <summary>
    /// 所使用的数据服务
    /// </summary>
    protected GameDataService DataService { get; private set; }


    /// <summary>
    /// 地块坐标
    /// </summary>
    public Coordinate Coordinate { get; private set; }

    /// <summary>
    /// 地块上的建筑
    /// </summary>
    public abstract BuildingDescriptor Building { get; set; }


    /// <summary>
    /// 地块上存在的资源
    /// </summary>
    public abstract ItemCollection Resources { get; }


    /// <summary>
    /// 地块的拥有者
    /// </summary>
    public abstract GamePlayer Owner { get; set; }

    /// <summary>
    /// 正在进行的活动
    /// </summary>
    public abstract PlaceActing Acting { get; set; }


    /// <summary>
    /// 上次检查时间
    /// </summary>
    public abstract DateTime CheckPoint { get; set; }



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
        Owner.Resources.Collect( Resources );
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
        IsMine = Owner == player,

        Nearly = coordinate.NearlyCoordinates(),
      } );

      if ( Acting == null )
        data["Actions"] = JArray.FromObject( GetActions().Select( item => item.GetInfo() ) );

      else
        data["Action"] = JObject.FromObject( Acting.GetInfo() );

      return data;
    }



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




    public static bool operator ==( Place a, Place b )
    {

      if ( object.ReferenceEquals( a, null ) && object.ReferenceEquals( b, null ) )
        return true;

      if ( object.ReferenceEquals( a, null ) || object.ReferenceEquals( b, null ) )
        return false;

      return a.Equals( b );
    }


    public static bool operator !=( Place a, Place b )
    {

      if ( object.ReferenceEquals( a, null ) && object.ReferenceEquals( b, null ) )
        return false;

      if ( object.ReferenceEquals( a, null ) || object.ReferenceEquals( b, null ) )
        return true;

      return a.Equals( b ) == false;
    }






    /// <summary>
    /// 获取可以执行的活动列表
    /// </summary>
    /// <returns></returns>
    public virtual ActionDescriptor[] GetActions()
    {
      return Building.GetActions();
    }
  }
}
