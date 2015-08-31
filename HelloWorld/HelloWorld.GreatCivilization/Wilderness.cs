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



    private static readonly Guid forestId = new Guid( "8105A10F-1E11-4FF0-B2D2-1FD910258648" );
    private static readonly Guid grasslandId = new Guid( "C6E4329A-D722-439C-B846-F0EADF388AD9" );


    public override void Check( Place place )
    {

      for ( int i = 0; i < ( DateTime.UtcNow - place.CheckPoint ).Minutes; i++ )
      {
        if ( Probability.IfHit( 1 / 100d, () => GameHost.GameRules.GetDataItem<BuildingDescriptor>( forestId ) ) )
          break;

        if ( Probability.IfHit( 1 / 100d, () => GameHost.GameRules.GetDataItem<BuildingDescriptor>( grasslandId ) ) )
          break;
      }

      base.Check( place );
    }


    public new static Wilderness FromData( Guid guid, JObject data )
    {

      var building = BuildingDescriptor.FromData( guid, data );

      if ( building == null )
        return null;

      return new Wilderness( building );
    }



  }
}
