using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{



  public class ResourceRequirment
  {

    public ItemList Items { get; private set; }

    public TimeSpan Time { get; private set; }

    public int Workers { get; private set; }



  }
}
