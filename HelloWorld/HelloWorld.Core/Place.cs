using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public abstract GameActing Acting { get; set; }




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
      return new
      {
        Coordinate = Coordinate - player.Initiation,
        Building = Building.GetInfo(),
        Resources,
        Acting = Acting == null ? null : Acting.GetInfo(),
        Actions = GameHost.GameRules.GetActions( this ).Select( item => item.GetInfo() ),
        IsMine = Owner == player,
      };
    }



    public override bool Equals( object obj )
    {
      var place = obj as Place;

      if ( place == null )
        return false;


      return place.Coordinate == this.Coordinate;
    }

    public override int GetHashCode()
    {
      return Coordinate.GetHashCode();
    }


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





  }
}
