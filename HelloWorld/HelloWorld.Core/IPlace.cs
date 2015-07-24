using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{
  /// <summary>
  /// 代表一个地块
  /// </summary>
  public interface IPlace
  {

    /// <summary>
    /// 地块坐标
    /// </summary>
    Coordinate Coordinate { get; }

    /// <summary>
    /// 地块上的建筑
    /// </summary>
    BuildingDescriptor Building { get; }

  }
}
