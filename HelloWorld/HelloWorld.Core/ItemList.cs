using Newtonsoft.Json;
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
  [JsonConverter( typeof( ItemListJsonConverter ) )]
  public sealed class ItemList : ReadOnlyCollection<Item>
  {


    internal ItemList( Item[] data ) : base( data ) { }



    public override string ToString()
    {
      return ItemListJsonConverter.ToJson( this ).ToString();
    }



    public static implicit operator ItemList( Item[] items )
    {
      return new ItemList( items );
    }

  }
}
