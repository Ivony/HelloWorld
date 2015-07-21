using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{
  public class JsonDataHost : GameDataHost
  {

    public JsonDataHost( string dataRoot )
    {
      DataRoot = new DirectoryInfo( dataRoot );
      DataRoot.Create();
    }

    public DirectoryInfo DataRoot
    {
      get; private set;
    }

    public override Place GetPlace( Coordinate coordinate )
    {
      throw new NotImplementedException();
    }

    public override Player GetPlayer( Guid userId )
    {
      throw new NotImplementedException();
    }

    public override Player RegisterPlayer( string email, string password )
    {
      throw new NotImplementedException();
    }
  }
}
