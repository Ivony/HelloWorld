using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{
  /// <summary>
  /// 代表一个地块
  /// </summary>
  public sealed class Place
  {

    /// <summary>
    /// 地块坐标
    /// </summary>
    public Coordinate Coordinate { get; private set; }

    /// <summary>
    /// 地块建筑
    /// </summary>
    public BuildingDescriptor Building { get; private set; }

  }
}
