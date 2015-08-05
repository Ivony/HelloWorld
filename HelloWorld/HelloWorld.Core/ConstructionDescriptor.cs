using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  /// <summary>
  /// 定义一个建造过程描述
  /// </summary>
  public class ConstructionDescriptor
  {

    /// <summary>
    /// 原建筑
    /// </summary>
    public BuildingDescriptor OriginBuilding { get; private set; }


    /// <summary>
    /// 新建筑
    /// </summary>
    public BuildingDescriptor NewBuiding { get; private set; }


    /// <summary>
    /// 生产过程所需资源描述
    /// </summary>
    public ResourceRequirment ResourceRequirment { get; private set; }

  }
}
