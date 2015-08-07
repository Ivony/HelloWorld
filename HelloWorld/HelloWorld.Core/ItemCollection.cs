using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个物品容器
  /// </summary>
  public class ItemCollection
  {


    public ItemCollection() { }


    public void AddItems( Item item )
    {
      AddItems( item.ItemDescriptor, item.Quantity );
    }

    public void AddItems( ItemDescriptor item, int quantity )
    {

    }

    public void RemoveItems( ItemDescriptor item, int quantity )
    {

    }

    public ItemList Items
    {
      get { throw new NotImplementedException(); }
    }


  }
}
