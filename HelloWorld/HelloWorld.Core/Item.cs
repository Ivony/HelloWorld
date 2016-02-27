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
  public class Item : GameDataItem
  {


    /// <summary>
    /// 创建 Item 对象
    /// </summary>
    /// <param name="descriptor">物品描述</param>
    /// <param name="quantity">物品数量</param>
    public Item( ItemDescriptor descriptor, int quantity )
    {
      ItemDescriptor = descriptor;
      Quantity = quantity;
    }



    public Item() { }


    internal void InitializeData( ItemCollection collection, JsonDataObject data )
    {
      base.InitializeData( collection.DataService, collection, data );
    }






    /// <summary>
    /// 物品描述
    /// </summary>
    public ItemDescriptor ItemDescriptor { get; private set; }

    /// <summary>
    /// 物品数量
    /// </summary>
    public int Quantity { get; private set; }



    /// <summary>
    /// 获取物品唯一标识，默认情况下是物品描述的 GUID
    /// </summary>
    public virtual Guid Identifier { get { return ItemDescriptor.Guid; } }



    /// <summary>
    /// 重写 ToString 方法，返回物品名称和数量
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return JObject.FromObject( new Dictionary<string, string>() { { ItemDescriptor.Expression, Quantity.ToString() } } ).ToString();
    }


    /// <summary>
    /// 比较两个物品对象
    /// </summary>
    /// <param name="obj">要比较的物品对象</param>
    /// <returns></returns>
    public override bool Equals( object obj )
    {

      var item2 = obj as Item;

      if ( item2 == null )
        return false;


      return item2.Identifier == Identifier && item2.Quantity == Quantity;
    }


    /// <summary>
    /// 重写 GetHashCode 方法，获取哈希值
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
      return unchecked(Identifier.GetHashCode() + Quantity);
    }



  }
}
