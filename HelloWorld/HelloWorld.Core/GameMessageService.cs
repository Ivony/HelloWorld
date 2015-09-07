using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{


  /// <summary>
  /// 定义游戏消息服务
  /// </summary>
  public abstract class GameMessageService
  {
    public abstract void SendMessage( Guid playerId, GameMessageEntry message );


    public abstract void SendAnnouncement( GameMessageEntry message );


    public abstract GameMessageEntry[] GetMessages( Guid playerId, DateTime startTime );

  }
}
