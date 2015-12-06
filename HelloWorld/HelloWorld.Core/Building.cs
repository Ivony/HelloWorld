using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个实际存在的建筑
  /// </summary>
  public class Building
  {

    /// <summary>
    /// 建筑物描述对象
    /// </summary>
    public BuildingDescriptor BuildingDescriptor { get; private set; }

    /// <summary>
    /// 建筑物所在地块
    /// </summary>
    public Place Place { get; private set; }


    /// <summary>
    /// 检查建筑物是否满足建筑物限制
    /// </summary>
    /// <param name="restriction"></param>
    /// <returns></returns>
    public bool IsSatisfy( BuildingRestriction restriction )
    {
      return BuildingDescriptor == restriction.Building;
    }

  }
}
