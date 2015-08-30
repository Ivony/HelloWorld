using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWorld
{
  public class ItemListJsonConverter : JsonConverter
  {
    public override bool CanConvert( Type objectType )
    {
      return objectType == typeof( ItemList ) || objectType == typeof( ItemCollection );
    }

    public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
    {

      var data = serializer.Deserialize<JObject>( reader );

      var items = FromJson( data );

      if ( objectType == typeof( ItemList ) )
        return new ItemList( items );

      else if ( objectType == typeof( ItemCollection ) )
        return new ItemCollection( items );

      else
        throw new NotSupportedException();
    }

    private static Item CreateItem( JProperty property )
    {
      var index = property.Name.IndexOf( "/" );
      string id;
      if ( index > 0 )
        id = property.Name.Remove( index );
      else
        id = property.Name;

      return new Item( GameHost.GameRules.GetDataItem<ItemDescriptor>( Guid.Parse( id ) ), property.Value.Value<int>() );
    }

    public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
    {
      serializer.Serialize( writer, ToJson( (IEnumerable<Item>) value ) );
    }

    /// <summary>
    /// 从 JSON 数据中加载
    /// </summary>
    /// <param name="data">JSON 数据</param>
    /// <returns></returns>
    public static Item[] FromJson( JObject data )
    {
      if ( data == null )
        return new Item[0];

      return data.Properties().Select( p => CreateItem( p ) ).ToArray();
    }

    /// <summary>
    /// 转换为 JSON 数据对象
    /// </summary>
    /// <param name="list">数据源</param>
    /// <returns></returns>
    internal static JObject ToJson( IEnumerable<Item> list )
    {
      var data = new JObject();

      if ( list != null )
      {
        foreach ( var item in list )
          data.Add( item.ItemDescriptor.Expression, item.Quantity );
      }

      return data;
    }
  }
}