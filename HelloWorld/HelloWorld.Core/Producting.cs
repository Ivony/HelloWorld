using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class Producting : GameActing<ProductionDescriptor>
  {

    public Producting( ProductionDescriptor production ) : base( production ) { }



    public ProductionDescriptor Production { get { return base.Descriptor; } }

    protected override void Complete()
    {

    }
  }
}