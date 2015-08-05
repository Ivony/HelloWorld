using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个生产过程
  /// </summary>
  public sealed class ProductionDescription
  {


    /// <summary>
    /// 此生产过程所依赖的建筑/场所
    /// </summary>
    public BuildingDescriptor Building { get; private set; }


    /// <summary>
    /// 此生产过程所需的时间
    /// </summary>
    public TimeSpan RequiredTime { get; private set; }


    /// <summary>
    /// 此生产过程所需的劳工数量
    /// </summary>
    public int Workers { get; private set; }

  }
}
