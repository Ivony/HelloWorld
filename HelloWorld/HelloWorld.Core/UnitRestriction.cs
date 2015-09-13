using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 描述一个单位需求
  /// </summary>
  public class UnitRestriction
  {

    /// <summary>
    /// 单位描述
    /// </summary>
    public UnitDescriptor Unit { get; private set; }

    /// <summary>
    /// 单位限制
    /// </summary>
    public object Restrict { get; private set; }

  }
}
