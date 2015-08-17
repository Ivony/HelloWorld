﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 代表正在进行的一个活动
  /// </summary>
  public abstract class GameActing
  {

    protected GameActing( DateTime startOn, Place place )
    {

      StartOn = startOn;
      Place = place;

    }


    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartOn { get; private set; }

    /// <summary>
    /// 所在位置
    /// </summary>
    public Place Place { get; private set; }

    /// <summary>
    /// 活动状态
    /// </summary>
    public abstract GameActingStatus Status { get; }


    /// <summary>
    /// 完成这个活动
    /// </summary>
    protected abstract void Complete();


  }



  /// <summary>
  /// 定义游戏活动状态
  /// </summary>
  public sealed class GameActingStatus
  {

    /// <summary>
    /// 正在进行
    /// </summary>
    public static readonly GameActingStatus Processing = new GameActingStatus( "Processing" );

    /// <summary>
    /// 已经完成
    /// </summary>
    public static readonly GameActingStatus Done = new GameActingStatus( "Done" );


    private string status;

    private GameActingStatus( string status )
    {
      this.status = status;
    }


    public override bool Equals( object obj )
    {
      var a = obj as GameActingStatus;
      if ( a == null )
        return false;


      return a.status == status;
    }


    public override int GetHashCode()
    {
      return status.GetHashCode();
    }

    public override string ToString()
    {
      return status;
    }

  }
}