using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.GreatCivilization
{
  public class BuildFactory : ActionDescriptor
  {

    protected override bool CanStartAt( Place place )
    {
      var worker = GameHost.GameRules.GetDataItem<UnitDescriptor>( new Guid( "{72213162-0D16-4C53-89F3-AE2A0180E031}" ) );


      throw new NotImplementedException();
    }

  }
}
