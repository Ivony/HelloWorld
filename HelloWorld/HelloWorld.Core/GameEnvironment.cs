using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{


  /// <summary>
  /// 游戏全局规则和对象管理器
  /// </summary>
  public static class GameEnvironment
  {

    /// <summary>
    /// 获取一个物品描述
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static ItemDescriptor GetItem( string name )
    {
      throw new NotImplementedException();
    }


    /// <summary>
    /// 获取一个建筑描述
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static BuildingDescriptor GetBuilding( string name )
    {
      throw new NotImplementedException();
    }


    /// <summary>
    /// 获取一条生产规则
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static ProductionDescription GetProduction( string name )
    {
      throw new NotImplementedException();
    }

  }
}
