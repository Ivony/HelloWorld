using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个物品清单
  /// </summary>
  public sealed class ItemList : ReadOnlyCollection<Item>
  {


    internal ItemList( Item[] data ) : base( data ) { }



    public static ItemList FromData( JArray data )
    {
      return new ItemList( data.Cast<JObject>().Select( item => new Item( GameEnvironment.GetItem( item.GuidValue( "Item" ) ), item.Value<int>( "Quantity" ) ) ).ToArray() );

    }
  }
}
