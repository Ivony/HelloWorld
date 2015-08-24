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


    /// <summary>
    /// 活动完成
    /// </summary>
    protected override void Complete()
    {
      lock ( Place.SyncRoot )
      {
        if ( Place.Producting != this )
          throw new InvalidOperationException();

        if ( Place.Owner == null )
          throw new InvalidOperationException();

        Place.Resources.AddItems( Production.Output );
        Place.Producting = null;
        Place.Owner.Workers += Production.Requirment.Workers;

        Dispose();
      }
    }
  }
}