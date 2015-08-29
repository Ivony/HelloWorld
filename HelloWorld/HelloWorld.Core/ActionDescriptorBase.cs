using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{


  /// <summary>
  /// 一个游戏活动的描述
  /// </summary>
  public abstract class ActionDescriptorBase : GameDataItem
  {


    protected ActionDescriptorBase( Guid guid, JObject data ) : base( guid, data ) { }

    /// <summary>
    /// 尝试对指定地块进行投入来进行这个活动
    /// </summary>
    /// <param name="place">进行活动的地块</param>
    /// <returns>是否成功</returns>
    public abstract GameActing TryStartAt( Place place );


    /// <summary>
    /// 检查活动是否已经完成
    /// </summary>
    /// <param name="acting">正在进行的活动对象</param>
    /// <returns>是否已经完成</returns>
    public abstract bool TryComplete( GameActing acting );

  }
}
