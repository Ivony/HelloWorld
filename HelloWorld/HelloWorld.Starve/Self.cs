using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Starve
{
  public sealed class Self : Unit
  {

    public override decimal MobilityRequired( Place place )
    {
      return 0;
    }

  }
}
