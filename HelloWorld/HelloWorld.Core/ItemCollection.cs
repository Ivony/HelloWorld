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
  [JsonConverter( typeof( ItemListJsonConverter ) )]
  public class ItemCollection : IEnumerable<Item>
  {



    private Dictionary<ItemDescriptor, int> _collection = new Dictionary<ItemDescriptor, int>();

    private object _sync = new object();


    public ItemCollection() { }



    public ItemCollection( Item[] data )
    {
      foreach ( var i in data )
        AddItems( i );
    }


    /// <summary>
    /// 初始化一个物品容器
    /// </summary>
    /// <param name="data">物品数据</param>
    /// <param name="changeHandler">更改处理器</param>
    public ItemCollection( Item[] data, Action changeHandler ) : this( data )
    {
      ChangeHandler = changeHandler;
    }


    protected Action ChangeHandler { get; private set; }



    public void AddItems( Item item )
    {
      AddItems( item.ItemDescriptor, item.Quantity );
    }


    public void AddItems( ItemList items )
    {
      foreach ( var i in items )
        AddItems( i );
    }



    /// <summary>
    /// 收集另一个物品容器中的所有物品，并清空他。
    /// </summary>
    /// <param name="items"></param>
    public void Collect( ItemCollection items )
    {

      lock ( items._sync )
      {
        lock ( _sync )
        {
          foreach ( var item in items )
            AddItems( item );

          items.Clear();
        }
      }
    }


    public void AddItems( ItemDescriptor item, int quantity )
    {

      lock ( _sync )
      {
        if ( _collection.ContainsKey( item ) )
          _collection[item] += quantity;

        else
          _collection.Add( item, quantity );

        if ( ChangeHandler != null )
          ChangeHandler();
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

        if ( ChangeHandler != null )
          ChangeHandler();
      }
    }

    IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
    {
      return ((ItemList) this).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((ItemList) this).GetEnumerator();
    }


    public static implicit operator ItemList( ItemCollection items )
    {
      lock ( items._sync )
      {
        return new ItemList( items._collection.Select( item => new Item( item.Key, item.Value ) ).ToArray() );
      }
    }


    /// <summary>
    /// 清空物品容器
    /// </summary>
    public void Clear()
    {
      lock ( _sync )
      {
        _collection.Clear();
      }
    }

  }
}
