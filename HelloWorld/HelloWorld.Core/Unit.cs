﻿using System;
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
  public class Unit : GameDataItem
  {
    public Unit( IGameDataService dataService, Guid id, UnitDescriptor unit, Coordinate coordinate ) : base( dataService )
    {

      DataObject.ID = id;
      DataObject.Descriptor = unit.Guid;
      DataObject.Coordinate = coordinate.ToString();
      DataObject.State = UnitActionState.Idle;
      DataObject.Mobility = 0m;
      DataObject.LastActTime = DateTime.UtcNow;

      Initialze();
    }


    private Unit( IGameDataService dataService, JObject data ) : base( dataService )
    {
      JsonObject.Merge( data );
      Initialze();
    }


    protected virtual void Initialze()
    {
      Guid = JsonObject.GuidValue( "ID" ).Value;
      UnitDescriptor = GameHost.GameRules.GetDataItem<UnitDescriptor>( JsonObject.GuidValue( "Descriptor" ) );
    }



    /// <summary>
    /// 单位唯一标识
    /// </summary>
    public Guid Guid { get; private set; }


    /// <summary>
    /// 单位描述对象
    /// </summary>
    public UnitDescriptor UnitDescriptor { get; private set; }


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
    public void Check()
    {

      if ( ActionState == UnitActionState.Idle )
      {

        RecoveryMobilityForIdle();

      }
    }


    /// <summary>
    /// 如果休息时间足够，则复原移动力
    /// </summary>
    private void RecoveryMobilityForIdle()
    {
      var restTime = DateTime.UtcNow - LastActTime;

      while ( Mobility < UnitDescriptor.MobilityMaximum )
      {
        restTime -= UnitDescriptor.MobilityRecoveryCycle;
        if ( restTime < TimeSpan.Zero )
          break;

        Mobility += UnitDescriptor.MobilityRecoveryScale;
        if ( Mobility > UnitDescriptor.MobilityMaximum )
        {
          Mobility = UnitDescriptor.MobilityMaximum;
          break;
        }
      }
    }


    public ActionDescriptor[] GetActions()
    {

      return UnitDescriptor.GetActions( this );

    }



    public Coordinate Coordinate { get; private set; }


    public bool IsSatisfy( UnitRestriction restriction )
    {
      return UnitDescriptor == restriction.Unit;
    }



    /// <summary>
    /// 获取移动到指定位置所需要的移动力
    /// </summary>
    /// <param name="place">抵达位置</param>
    /// <returns>所需移动力</returns>
    public decimal MobilityRequired( Place place )
    {

      return 1;

    }




    /// <summary>
    /// 移动单位
    /// </summary>
    /// <param name="direction">移动方向</param>
    /// <returns>是否成功</returns>
    public bool Move( Direction direction )
    {

      Check();


      var target = GameHost.DataService.GetPlace( Coordinate.GetCoordinate( direction ) );



      lock ( SyncRoot )
      {

        var m = MobilityRequired( target );

        if ( Mobility < m || m == -1 )
          return false;


        Coordinate = target.Coordinate;
        Mobility -= m;
        LastActTime = DateTime.UtcNow;


        Save();
        return true;
      }
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
