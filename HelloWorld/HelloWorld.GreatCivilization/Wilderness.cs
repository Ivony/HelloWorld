using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld.GreatCivilization
{
  public class Wilderness : BuildingDescriptor
  {


    private Wilderness( BuildingDescriptor building ) : base( building ) { }


    public override void Check( Place place )
    {

    }


    public static Wilderness FromData( Guid guid, JObject data )
    {

      var building = BuildingDescriptor.FromData( guid, data );

      if ( building == null )
        return null;

      return new Wilderness( building );
    }



  }
}
