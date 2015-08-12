using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class Producting : GameActing
  {

    public Producting( DateTime startOn, Place place, ProductionDescription production ) : base( startOn, place )
    {
      Production = production;
    }



    public override GameActingStatus Status { get { return GameActingStatus.Processing; } }


    public ProductionDescription Production { get; private set; }

    protected override void Complete()
    {
      
    }
  }
}