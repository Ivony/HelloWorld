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

    public BuildingDescriptor BuildingDescriptor { get; private set; }

    public Place Place { get; private set; }


    public bool IsSatisfy( BuildingRestriction restriction )
    {
      return BuildingDescriptor == restriction.Building;
    }

  }
}
