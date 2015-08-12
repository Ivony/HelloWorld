using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{



  public class ResourceRequirment
  {

    public static ResourceRequirment FromData( JObject data )
    {
      return new ResourceRequirment
      {
        Items = ItemListJsonConverter.FromJson( (JObject) data["Items"] ),
        Time = data.TimeValue( "Time" ),
        Workers = data.Value<int>( "Workers" ),
      };
    }


    public ItemList Items { get; private set; }

    public TimeSpan Time { get; private set; }

    public int Workers { get; private set; }



  }
}
