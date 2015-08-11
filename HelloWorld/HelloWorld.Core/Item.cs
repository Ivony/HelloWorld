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


    public override string ToString()
    {
      return JObject.FromObject( new Dictionary<string, string>() { { ItemDescriptor.Expression, Quantity.ToString() } } ).ToString();
    }


    public override bool Equals( object obj )
    {

      var item2 = obj as Item;

      if ( item2 == null )
        return false;


      return item2.ItemDescriptor == ItemDescriptor && item2.Quantity == Quantity;
    }


    public override int GetHashCode()
    {
      return unchecked(ItemDescriptor.GetHashCode() + Quantity);
    }



  }
}
