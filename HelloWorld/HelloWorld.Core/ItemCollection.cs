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



    /// <summary>
    /// 初始化一个物品容器
    /// </summary>
    /// <param name="data">物品数据</param>
    /// <param name="changeHandler">更改处理器</param>
    public ItemCollection( Item[] data, Action<ItemCollection> changeHandler = null )
    {
      foreach ( var i in data )
        AddItemsInternal( i.ItemDescriptor, i.Quantity );


      ChangeHandler = changeHandler;
    }



    private void OnChanged()
    {
      if ( ChangeHandler != null )
        ChangeHandler( this );
    }


    protected Action<ItemCollection> ChangeHandler { get; private set; }



    /// <summary>
    /// 添加物品项
    /// </summary>
    /// <param name="items">要添加的物品项</param>
    public void AddItems( ItemList items )
    {
      if ( items == null )
        return;

      lock ( _sync )
      {
        foreach ( var i in items )
          AddItemsInternal( i.ItemDescriptor, i.Quantity );

        OnChanged();
      }
    }


    /// <summary>
    /// 收集另一个物品容器中的所有物品，并清空他。
    /// </summary>
    /// <param name="collection">物品容器</param>
    public void Collect( ItemCollection collection )
    {
      if ( collection == null )
        throw new ArgumentNullException( "collection" );

      lock ( collection._sync )
      {
        lock ( _sync )
        {
          foreach ( var item in collection )
            AddItemsInternal( item.ItemDescriptor, item.Quantity );

          collection.Clear();
          OnChanged();
        }
      }
    }


    /// <summary>
    /// 添加物品项
    /// </summary>
    /// <param name="item">要添加的物品项</param>
    public void AddItems( Item item )
    {
      if ( item == null )
        return;

      AddItems( item.ItemDescriptor, item.Quantity );
    }


    public void AddItems( ItemDescriptor item, int quantity )
    {

      if ( item == null )
        throw new ArgumentNullException( "item" );

      if ( quantity < 0 )
        throw new ArgumentOutOfRangeException( "quantity" );

      if ( quantity == 0 )
        return;


      lock ( _sync )
      {
        AddItemsInternal( item, quantity );

        OnChanged();
      }
    }

    private void AddItemsInternal( ItemDescriptor item, int quantity )
    {
      if ( item == null )
        throw new ArgumentNullException( "item" );

      if ( quantity <= 0 )
        throw new ArgumentOutOfRangeException( "quantity" );


      if ( _collection.ContainsKey( item ) )
        _collection[item] += quantity;

      else
        _collection.Add( item, quantity );

    }





    /// <summary>
    /// 尝试移除指定物品列表
    /// </summary>
    /// <param name="items">要移除的物品列表</param>
    /// <returns>是否成功</returns>
    public bool RemoveItems( ItemList items )
    {

      if ( items == null )
        return true;

      lock ( _sync )
      {

        if ( items.All( i => this._collection.ContainsKey( i.ItemDescriptor ) && this._collection[i.ItemDescriptor] >= i.Quantity ) )
        {
          foreach ( var i in items )
            RemoveItemsInternal( i.ItemDescriptor, i.Quantity );

          OnChanged();
          return true;
        }

        else
          return false;
      }
    }



    /// <summary>
    /// 尝试移除指定的物品
    /// </summary>
    /// <param name="item">要移除的物品</param>
    /// <returns>是否成功</returns>
    public bool RemoveItems( Item item )
    {
      if ( item == null )
        return true;

      return RemoveItems( item.ItemDescriptor, item.Quantity );
    }


    /// <summary>
    /// 尝试移除指定的物品
    /// </summary>
    /// <param name="item">要移除的物品</param>
    /// <returns>是否成功</returns>
    public bool RemoveItems( ItemDescriptor item, int quantity )
    {
      if ( item == null )
        throw new ArgumentNullException( "item" );

      if ( quantity < 0 )
        throw new ArgumentOutOfRangeException( "quantity" );

      if ( quantity == 0 )
        return true;


      lock ( _sync )
      {
        if ( RemoveItemsInternal( item, quantity ) )
        {
          OnChanged();
          return true;
        }

        else
          return false;
      }
    }


    private bool RemoveItemsInternal( ItemDescriptor item, int quantity )
    {
      if ( item == null )
        throw new ArgumentNullException( "item" );

      if ( quantity <= 0 )
        throw new ArgumentOutOfRangeException( "quantity" );

      if ( _collection.ContainsKey( item ) && _collection[item] < quantity )
        return false;

      _collection[item] -= quantity;
      return true;
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
