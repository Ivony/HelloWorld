using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{

  /// <summary>
  /// 代表正在进行的一个建造过程
  /// </summary>
  public class Constructing : GameActing
  {

    public Constructing( DateTime startOn, Place place, ConstructionDescriptor construction ) : base( startOn, place )
    {
      Construction = construction;
    }


    public ConstructionDescriptor Construction { get; private set; }



    public override GameActingStatus Status
    {
      get { return GameActingStatus.Processing; }
    }

    protected override void Complete()
    {
      Place.Building = Construction.NewBuiding;
    }
  }
}
