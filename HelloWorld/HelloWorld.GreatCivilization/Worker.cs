using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.GreatCivilization
{
  public class Worker : Unit
  {


    public override decimal MobilityRequired( Place place )
    {
      if ( place.Owner == this.Owner )//在自己的领土上移动不需要移动力
        return 0;

      return base.MobilityRequired( place );
    }


  }
}
