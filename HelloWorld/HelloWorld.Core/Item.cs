using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个物品资源
  /// </summary>
  public class Item
  {


    public Item( ItemDescriptor descriptor, int quantity )
    {
      ItemDescriptor = descriptor;
      Quantity = quantity;
    }



    public ItemDescriptor ItemDescriptor { get; private set; }

    public int Quantity { get; private set; }

  }
}
