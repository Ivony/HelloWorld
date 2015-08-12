using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class CoordinateJsonConverter : JsonConverter
  {
    public override bool CanConvert( Type objectType )
    {
      return objectType == typeof( Coordinate );
    }

    public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
    {
      if ( objectType != typeof( Coordinate ) )
        throw new NotSupportedException();

      var str = serializer.Deserialize<string>( reader );

      return Coordinate.Parse( str );
    }

    public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
    {

      serializer.Serialize( writer, ((Coordinate) value).ToString() );
    }
  }
}
