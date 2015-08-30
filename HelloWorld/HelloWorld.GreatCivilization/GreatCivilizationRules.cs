using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.GreatCivilization
{
  public class GreatCivilizationRules : GameRules
  {

    public GreatCivilizationRules( ITypeResolver typeResolver )
      : base( new ResourceDataResolver( Assembly.GetExecutingAssembly() ), typeResolver )
    {


    }


    public override void Initialize()
    {
      base.Initialize();

      _initiationBuilding = GetDataItem<BuildingDescriptor>( new Guid( "eb0c8ae8-fc09-4874-9985-98c081f4d1b7" ) );
    }

    BuildingDescriptor _initiationBuilding;

    public override BuildingDescriptor InitiationBuilding
    {
      get { return _initiationBuilding; }
    }

  }
}
