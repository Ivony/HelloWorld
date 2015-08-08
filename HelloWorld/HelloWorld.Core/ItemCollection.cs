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



    private Dictionary<ItemDescriptor, int> _collection = new Dictionary<ItemDescriptor, int>();

    private object _sync = new object();


    public ItemCollection() { }


    public void AddItems( Item item )
    {
      AddItems( item.ItemDescriptor, item.Quantity );
    }

    public void AddItems( ItemDescriptor item, int quantity )
    {

      lock ( _sync )
      {
        if ( _collection.ContainsKey( item ) )
          _collection[item] += quantity;

        else
          _collection.Add( item, quantity );
      }
    }

    public void RemoveItems( ItemDescriptor item, int quantity )
    {
      lock ( _sync )
      {
        if ( _collection.ContainsKey( item ) && _collection[item] >= quantity )
          _collection[item] -= quantity;

        else
          throw new InvalidOperationException();
      }
    }

    public ItemList Items
    {
      get
      {
        lock ( _sync )
        {
          return new ItemList( _collection.Select( item => new Item( item.Key, item.Value ) ).ToArray() );
        }
      }
    }
  }
}
