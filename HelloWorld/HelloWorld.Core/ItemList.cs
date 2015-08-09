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
  [JsonConverter( typeof( ItemList.ItemListTypeConverter ) )]
  public sealed class ItemList : ReadOnlyCollection<Item>
  {


    internal ItemList( Item[] data ) : base( data ) { }



    public static ItemList FromData( JArray data )
    {
      return new ItemList( data.Cast<JObject>().Select( item => new Item( GameEnvironment.GetItem( item.GuidValue( "Item" ) ), item.Value<int>( "Quantity" ) ) ).ToArray() );
    }

    public override string ToString()
    {
      return JObject.FromObject( this.ToDictionary( item => item.ItemDescriptor.Expression, item => item.Quantity ) ).ToString();
    }



    public class ItemListTypeConverter : JsonConverter
    {
      public override bool CanConvert( Type objectType )
      {
        return objectType == typeof( ItemList );
      }

      public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
      {

        throw new NotSupportedException();

      }

      public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
      {

        var list = (ItemList) value;
        var data = new JObject();

        foreach ( var item in list )
          data.Add( item.ItemDescriptor.Guid.ToString( "B" ) + "/" + item.ItemDescriptor.Name, item.Quantity );

        serializer.Serialize( writer, data );
      }
    }

  }
}
