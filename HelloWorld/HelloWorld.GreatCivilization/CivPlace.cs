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

      if ( Terrain == null )
        SetTerrain( new Guid( "{A70F06B8-341E-4DB4-9D8C-13E073CD2E8B}" ) );

      if ( Building == null )
        SetBuilding( new Guid( "{EB0C8AE8-FC09-4874-9985-98C081F4D1B7}" ) );

    }


    public override ActionDescriptor[] GetActions()
    {
      return GreatCivilizationRules.Instance.GetActions();
    }

  }
}
