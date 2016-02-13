using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.GreatCivilization
{
  public class CivPlace : HelloWorld.Place
  {

    internal CivPlace( Coordinate coordinate ) : base( coordinate )
    {

    }


    protected override void Initialize()
    {
      base.Initialize();

      if ( Building == null )
        SetBuilding( GameHost.GameRules.GetDataItem<BuildingDescriptor>( new Guid( "eb0c8ae8-fc09-4874-9985-98c081f4d1b7" ) ) );

    }



    public override ActionDescriptor[] GetActions()
    {
      return new ActionDescriptor[0];
    }

  }
}
