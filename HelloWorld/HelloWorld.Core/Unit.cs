using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{


  /// <summary>
  /// 代表一个玩家的单位
  /// </summary>
  public class Unit : GameDataItem, IDisposable
  {



    public static Unit CreateUnit( IGameDataService dataService, UnitDescriptor descriptor, Guid owner, Coordinate coordinate, Guid id, string name )
    {
      var data = new JObject() as dynamic;

      data.ID = id;
      data.Name = name;
      data.Owner = owner;
      data.Descriptor = descriptor.Guid;
      data.Coordinate = coordinate.ToString();
      data.State = UnitActionState.Idle;
      data.Mobility = 0m;
      data.LastActTime = DateTime.UtcNow;



      var type = descriptor.InstanceType;
      var unit = (Unit) Activator.CreateInstance( type );

      unit.InitializeData( dataService, data );
      return unit;
    }



    /// <summary>
    /// 创建一个玩家单位对象
    /// </summary>
    /// <param name="dataService">游戏数据服务</param>
    /// <param name="data">单位数据</param>
    public Unit()
    {
    }



    /// <summary>
    /// 初始化对象
    /// </summary>
    protected override void Initialize()
    {
      Guid = JsonObject.GuidValue( "ID" ).Value;
      Name = (string) DataObject.Name;

      if ( Name == null )
        Name = DataService.NameService.AllocateName();


      Descriptor = GameHost.GameRules.GetDataItem<UnitDescriptor>( JsonObject.GuidValue( "Descriptor" ) );

      base.Initialize();
    }

    /// <summary>
    /// 获取用于输出给客户端的信息
    /// </summary>
    /// <returns></returns>
    public virtual object GetInfo()
    {
      return new
      {
        Guid,
        Name,
        Coordinate = Coordinate.ToRelative( GetPlayer() ),
        Mobility,
        ActionState = ActionState.ToString(),
        Descriptor = Descriptor.GetInfo(),
      };
    }



    /// <summary>
    /// 单位唯一标识
    /// </summary>
    public Guid Guid { get; private set; }



    /// <summary>
    /// 单位名字，用于称呼和标识单位
    /// </summary>
    public string Name { get; private set; }



    /// <summary>
    /// 更改单位名字
    /// </summary>
    /// <param name="name">新的名字</param>
    public virtual void Rename( string name )
    {
      DataObject.Name = Name = name;
    }


    /// <summary>
    /// 单位描述对象
    /// </summary>
    public UnitDescriptor Descriptor { get; private set; }




    /// <summary>
    /// 单位目前活动状态
    /// </summary>
    public UnitActionState ActionState
    {
      get { return DataObject.State; }
      private set { DataObject.State = value; }
    }


    /// <summary>
    /// 剩余的移动力
    /// </summary>
    public decimal Mobility
    {
      get { return DataObject.Mobility; }
      private set { DataObject.Mobility = value; }
    }

    /// <summary>
    /// 上次行动时间
    /// </summary>
    public DateTime LastActTime
    {
      get { return DataObject.LastActTime; }
      private set { DataObject.LastActTime = value; }
    }



    /// <summary>
    /// 例行检查状态
    /// </summary>
    public virtual void Check( DateTime now )
    {

      if ( ActionState == UnitActionState.Idle )
      {
        RecoveryMobilityForIdle( now );
      }

    }


    /// <summary>
    /// 如果休息时间足够，则复原移动力
    /// </summary>
    protected virtual void RecoveryMobilityForIdle( DateTime now )
    {
      var restTime = now - LastActTime;

      while ( Mobility < Descriptor.MobilityMaximum )
      {
        restTime -= Descriptor.MobilityRecoveryCycle;
        if ( restTime < TimeSpan.Zero )
          break;

        Mobility += Descriptor.MobilityRecoveryScale;
        if ( Mobility > Descriptor.MobilityMaximum )
        {
          Mobility = Descriptor.MobilityMaximum;
          break;
        }
      }
    }



    /// <summary>
    /// 获取当前单位可以进行的行动
    /// </summary>
    /// <returns></returns>
    public virtual ActionDescriptor[] GetActions()
    {

      return Descriptor.GetActions( this );

    }



    /// <summary>
    /// 获取单位所在的坐标
    /// </summary>
    public Coordinate Coordinate
    {
      get
      {
        return JsonObject.CoordinateValue( "Coordinate" );
      }

      private set
      {
        if ( value == null )
          throw new ArgumentNullException( "null" );

        JsonObject["Coordinate"] = value.ToString();
      }
    }


    /// <summary>
    /// 单位所有者 ID
    /// </summary>
    public Guid Owner
    {
      get { return JsonObject.GuidValue( "Owner" ).Value; }
    }



    /// <summary>
    /// 单位所有者玩家对象
    /// </summary>
    public GamePlayer GetPlayer() { return DataService.GetPlayer( Owner ); }



    /// <summary>
    /// 获取移动到指定位置所需要的移动力
    /// </summary>
    /// <param name="place">抵达位置</param>
    /// <returns>所需移动力</returns>
    public virtual decimal MobilityRequired( Place place )
    {

      return 1;

    }




    /// <summary>
    /// 移动单位
    /// </summary>
    /// <param name="direction">移动方向</param>
    /// <returns>是否成功</returns>
    public virtual bool Move( Direction direction )
    {

      var now = DateTime.UtcNow;
      Check( now );


      var target = GameHost.DataService.GetPlace( Coordinate.GetCoordinate( direction ) );



      lock ( SyncRoot )
      {

        var m = MobilityRequired( target );

        if ( Mobility < m || m == -1 )
          return false;


        Coordinate = target.Coordinate;
        Mobility -= m;
        LastActTime = now;


        Save();
        return true;
      }
    }


    /// <summary>
    /// 重写 Equals 方法比较两个单位对象的 GUID
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object obj )
    {
      var unit = obj as Unit;
      if ( unit == null )
        return false;

      return unit.Guid == this.Guid;
    }

    public override int GetHashCode()
    {
      return Guid.GetHashCode();
    }

    public void Dispose()
    {
    }
  }






  /// <summary>
  /// 定义单位的行动状态
  /// </summary>
  public enum UnitActionState
  {
    /// <summary>空闲</summary>
    Idle,
    /// <summary>正在执行某任务</summary>
    Acting
  }



}
