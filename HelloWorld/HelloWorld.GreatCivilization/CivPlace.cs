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
    }



    public override ActionDescriptor[] GetActions()
    {
      return new ActionDescriptor[0];
    }

  }
}
