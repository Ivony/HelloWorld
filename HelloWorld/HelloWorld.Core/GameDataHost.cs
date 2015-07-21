using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{
  public abstract class GameDataHost
  {
    public abstract Place GetPlace( Coordinate coordinate );


    public abstract Player GetPlayer( Guid userId );

  }
}
