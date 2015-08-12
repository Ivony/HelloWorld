using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class Producting : GameProgress
  {

    public Producting( DateTime startOn, Place place, ProductionDescription production ) : base( startOn, place )
    {
      Production = production;
    }



    public override GameProgressStatus Status { get { return GameProgressStatus.Processing; } }


    public ProductionDescription Production { get; private set; }
  }
}