using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{

  /// <summary>
  /// 定义一个物品资源
  /// </summary>
  public class Item
  {

    public ItemDescriptor ItemDescriptor { get; private set; }

    public int Quantity { get; private set; }

  }
}
