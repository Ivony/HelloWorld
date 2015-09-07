using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{
  public class GameMessageEntry
  {

    public GameMessageEntry( string content )
    {
      Content = content;
    }

    public string Content { get; private set; }

  }
}
