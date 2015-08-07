using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class Player
  {

    /// <summary>
    /// 昵称
    /// </summary>
    public abstract string NickName { get; set; }

    /// <summary>
    /// 起始位置
    /// </summary>
    public abstract Coordinate Initiation { get; }

  }
}
