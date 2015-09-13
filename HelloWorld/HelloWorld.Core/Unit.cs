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
  public class Unit
  {

    public Unit( Guid id, UnitDescriptor unit, Place place )
    {
      Guid = id;
      UnitDescriptor = unit;
      Place = place;
      ActionState = UnitActionState.Idle;
      Mobility = 0m;
      LastActTime = DateTime.UtcNow;
    }

    private Unit() { }



    public Guid Guid { get; private set; }

    public UnitDescriptor UnitDescriptor { get; private set; }


    public UnitActionState ActionState { get; protected set; }


    /// <summary>
    /// 剩余的移动力
    /// </summary>
    public decimal Mobility { get; private set; }

    /// <summary>
    /// 上次行动时间
    /// </summary>
    public DateTime LastActTime { get; private set; }




    public void Check()
    {

      if ( ActionState == UnitActionState.Idle )
      {
        //如果休息时间足够，则复原移动力
        if ( Mobility < UnitDescriptor.Mobility && DateTime.UtcNow - LastActTime > UnitDescriptor.RestTime )
          Mobility = UnitDescriptor.Mobility;
      }
    }


    public ActionDescriptor[] GetActions()
    {

      return UnitDescriptor.GetActions( this );

    }



    public Place Place { get; private set; }


    public static Unit FromData( Place place, JObject data )
    {

      if ( data == null )
        return null;

      var guid = (Guid) data.GuidValue( "Guid" );
      var unitDescriptor = GameHost.GameRules.GetDataItem<UnitDescriptor>( data.GuidValue( "UnitID" ) );
      var lastActTime = data.Value<DateTime>( "LastActTime" );
      var mobility = data.Value<decimal>( "Mobility" );

      return new Unit
      {
        Guid = guid,
        Place = place,
        UnitDescriptor = unitDescriptor,
        LastActTime = lastActTime,
        Mobility = mobility,
      };
    }

    public JObject ToJson()
    {
      return JObject.FromObject( new
      {
        Guid,
        UnitID = UnitDescriptor.Guid,
        LastActTime,
        Mobility,
      } );
    }



    public bool IsSatisfy( UnitRestriction restriction )
    {
      return UnitDescriptor == restriction.Unit;
    }


    /// <summary>
    /// 移动单位
    /// </summary>
    /// <param name="direction">移动方向</param>
    /// <returns>是否成功</returns>
    public bool Move( Direction direction )
    {
      return false;
    
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
