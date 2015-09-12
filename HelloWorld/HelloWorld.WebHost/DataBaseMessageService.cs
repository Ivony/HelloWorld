using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWorld.WebHost
{
  public class DataBaseMessageService : IGameMessageService
  {
    public void AddMessage( Guid playerId, GameMessageEntry message )
    {
      throw new NotImplementedException();
    }

    public void AddAnnouncement( GameMessageEntry message )
    {
      throw new NotImplementedException();
    }

    public IEnumerable<GameMessageEntry> GetMessages( Guid playerId, DateTime? startTime )
    {
      throw new NotImplementedException();
    }
  }
}