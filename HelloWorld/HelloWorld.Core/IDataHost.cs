using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个游戏数据的宿主对象
  /// </summary>
  internal interface IDataHost
  {



    /// <summary>
    /// 游戏数据对象唯一标识
    /// </summary>
    Guid Token { get; }

    /// <summary>
    /// 尝试保存数据对象
    /// </summary>
    /// <param name="data"></param>
    void Save( JToken data );

  }
}
