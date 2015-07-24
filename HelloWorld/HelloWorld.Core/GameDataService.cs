using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{
  public abstract class GameDataService
  {
    public abstract Task<Place> GetPlace( Coordinate coordinate );


    public abstract Task<IPlayer> GetPlayer( Guid userId );
  }
}
