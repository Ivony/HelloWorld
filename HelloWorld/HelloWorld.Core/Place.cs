﻿using System;
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

    protected Place()
    {
      SyncRoot = new object();
    }

    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    public object SyncRoot { get; private set; }

    /// <summary>
    /// 地块坐标
    /// </summary>
    public abstract Coordinate Coordinate { get; }

    /// <summary>
    /// 地块上的建筑
    /// </summary>
    public abstract BuildingDescriptor Building { get; set; }



    /// <summary>
    /// 正在进行的生产
    /// </summary>
    public abstract Producting Producting { get; set; }


    /// <summary>
    /// 正在进行的建造
    /// </summary>
    public abstract Constructing Constructing { get; set; }



    /// <summary>
    /// 地块上存在的资源
    /// </summary>
    public abstract ItemCollection Resources { get; }


    /// <summary>
    /// 收集地块上所有的资源
    /// </summary>
    /// <param name="player"></param>
    public void Collect( GamePlayer player )
    {
      lock ( SyncRoot )
      {
        player.Resources.Collect( Resources );
        Resources.Clear();
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
        Building,
        Resources,
        Constructing,
        Producting,
        Actions = new
        {
          Constructions = GameEnvironment.GetConstructions( Building ),
          Productions = GameEnvironment.GetProductions( Building ),
        }
      };
    }
  }
}
