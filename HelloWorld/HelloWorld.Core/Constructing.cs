using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{

  /// <summary>
  /// 代表正在进行的一个建造过程
  /// </summary>
  public class Constructing : GameActing<ConstructionDescriptor>
  {

    public Constructing( ConstructionDescriptor construction ) : base( construction ) { }


    public ConstructionDescriptor Construction { get { return base.Descriptor; } }


    protected override void Complete()
    {
      Place.Building = Construction.NewBuiding;
    }
  }
}
