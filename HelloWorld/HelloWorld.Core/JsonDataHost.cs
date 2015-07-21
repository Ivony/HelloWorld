using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{
  public class JsonDataHost : GameDataHost
  {
    public override Place GetPlace( Coordinate coordinate )
    {
      
    }

    public override Player GetPlayer( Guid userId )
    {
      throw new NotImplementedException();
    }
  }
}
