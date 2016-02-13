using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class TraficNetworkDescriptor : ImmovableDescriptor
  {

    protected override Type DefaultInstanceType
    {
      get { return typeof( TraficNetwork ); }
    }

  }
}
