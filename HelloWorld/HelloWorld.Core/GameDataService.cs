using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class GameDataService
  {
    public abstract Place GetPlace( Coordinate coordinate );

    public abstract GamePlayer GetPlayer( Guid userId );


  }
}
