using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个物品容器
  /// </summary>
  [JsonConverter( typeof( ItemListTypeConverter ) )]
  public class ItemCollection : IEnumerable<Item>
  {



    private Dictionary<ItemDescriptor, int> _collection = new Dictionary<ItemDescriptor, int>();

    private object _sync = new object();


    public ItemCollection() { }


    internal ItemCollection( Item[] data )
    {
      foreach ( var i in data )
        AddItems( i );
    }


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

    IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
    {
      return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return Items.GetEnumerator();
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
