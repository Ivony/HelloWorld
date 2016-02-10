using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.GreatCivilization
{
  public class NewWorker : ActionDescriptor
  {

    public override bool CanStartAt( Place place )
    {
      return base.CanStartAt( place );
    }

    public override PlaceActing TryStartAt( Place place )
    {
      return base.TryStartAt( place );
    }


    public override bool TryComplete( PlaceActing acting )
    {
      if ( base.TryComplete( acting ) == false )
        return false;

      acting.Place.NewUnit( new Guid( "{72213162-0D16-4C53-89F3-AE2A0180E031}" ) );

      return true;

    }

  }
}
