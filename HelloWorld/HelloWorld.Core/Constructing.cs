using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{

  /// <summary>
  /// 代表正在进行的一个建造过程
  /// </summary>
  public class Constructing : GameProgress
  {

    public Constructing( DateTime startOn, Place place, ConstructionDescriptor construction ) : base( startOn, place ) { }


    public ConstructionDescriptor Construction { get; private set; }



    public override GameProgressStatus Status
    {
      get { return GameProgressStatus.Processing; }
    }
  }
}
