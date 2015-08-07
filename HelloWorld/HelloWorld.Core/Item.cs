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


    private Item() { }

    internal static Item FromData( JObject data )
    {
      if ( data == null )
        return null;

      return new Item
      {
        ItemDescriptor = ItemDescriptor.FromData( (JObject) data["ItemDescriptor"] ),
        Quantity = (int) data["Quantity"],
      };
    }



    public ItemDescriptor ItemDescriptor { get; private set; }

    public int Quantity { get; private set; }

  }
}
