using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWorld
{
  public class ItemListTypeConverter : JsonConverter
  {
    public override bool CanConvert( Type objectType )
    {
      return objectType == typeof( ItemList ) || objectType == typeof( ItemCollection );
    }

    public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
    {

      var data = serializer.Deserialize<JObject>( reader );

      var items = data.Properties().Select( p => CreateItem( p ) ).ToArray();

      if ( objectType == typeof( ItemList ) )
        return new ItemList( items );

      else if ( objectType == typeof( ItemCollection ) )
        return new ItemCollection( items );

      else
        throw new NotSupportedException();
    }

    private Item CreateItem( JProperty property )
    {
      var id = Guid.Parse( property.Name.Remove( property.Name.IndexOf( "/" ) ) );
      return new Item( GameEnvironment.GetItem( id ), property.Value.Value<int>() );
    }

    public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
    {

      var list = (IEnumerable<Item>) value;
      var data = new JObject();

      foreach ( var item in list )
        data.Add( item.ItemDescriptor.Expression, item.Quantity );

      serializer.Serialize( writer, data );
    }
  }
}