﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 游戏宿主类型，用于获取游戏规则和游戏资源文件
  /// </summary>
  public static class GameHost
  {



    public static void Initialize( GameRules rules, string dataRoot )
    {
      if ( rules == null )
        throw new ArgumentNullException( "rules" );

      if ( dataRoot == null )
        throw new ArgumentNullException( "dataRoot" );

      Initialize( rules, new JsonDataService( dataRoot ) );
    }

    public static void Initialize( GameRules rules, GameDataService dataService )
    {
      if ( rules == null )
        throw new ArgumentNullException( "rules" );

      if ( dataService == null )
        throw new ArgumentNullException( "dataService" );

      GameRules = rules;
      DataService = dataService;

      GameRules.Initialize();
    }


    public static GameRules GameRules { get; private set; }

    public static GameDataService DataService { get; private set; }


    private static Random random = new Random( DateTime.Now.Millisecond );

    private static object _sync = new object();


    /// <summary>
    /// 获取一个小于指定整数的随机整数数
    /// </summary>
    /// <param name="maxValue">指定整数</param>
    /// <returns></returns>
    public static int Random( int maxValue )
    {
      lock ( _sync )
        return random.Next( maxValue );
    }
  }
}
