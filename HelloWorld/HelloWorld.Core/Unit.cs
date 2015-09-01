using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{


  /// <summary>
  /// 代表一个玩家的单位
  /// </summary>
  public class Unit
  {

    public UnitDescriptor UnitDescriptor { get; private set; }


    public Place Place { get; private set; }

  }
}
