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
  public interface IGameMessageService
  {
    void AddMessage( Guid playerId, GameMessageEntry message );


    void AddAnnouncement( GameMessageEntry message );


    GameMessageEntry[] GetMessages( Guid playerId, DateTime startTime );

  }
}
