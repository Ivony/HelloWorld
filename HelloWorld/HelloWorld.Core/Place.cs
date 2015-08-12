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

    /// <summary>
    /// 地块坐标
    /// </summary>
    public abstract Coordinate Coordinate { get; }

    /// <summary>
    /// 地块上的建筑
    /// </summary>
    public abstract BuildingDescriptor Building { get; set; }

    public object GetInfo( Player player )
    {
      return new
      {
        Coordinate = Coordinate - player.Initiation,
        Building,
      };
    }
  }
}
