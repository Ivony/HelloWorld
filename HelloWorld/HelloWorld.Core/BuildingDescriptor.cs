using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class BuildingDescriptor : ImmovableDescriptor
  {

    protected override Type DefaultInstanceType
    {
      get { return typeof( Building ); }
    }

  }
}
