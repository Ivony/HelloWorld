using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{
  public class GameMessageEntry
  {

    public GameMessageEntry( string content ) : this( DateTime.UtcNow, content ) { }


    public GameMessageEntry( DateTime notifyTime, string content )
    {
      NotifyTime = notifyTime;
      Content = content;

    }




    /// <summary>
    /// 产生消息通知的时间
    /// </summary>
    public DateTime NotifyTime { get; private set; }


    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; private set; }

  }
}
