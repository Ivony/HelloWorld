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
  public abstract class ActionDescriptorBase : GameRuleDataItem
  {



    public abstract string Name { get; }

    public abstract string Description { get; }


    protected ActionDescriptorBase() : base() { }


    /// <summary>
    /// 确认是否可以在指定地块开展活动
    /// </summary>
    /// <param name="place">要开展活动的地块</param>
    /// <returns>是否可以开展活动</returns>
    public abstract bool CanStartAt( Place place );


    /// <summary>
    /// 尝试对指定地块进行投入来进行这个活动
    /// </summary>
    /// <param name="place">进行活动的地块</param>
    /// <returns>是否成功</returns>
    public abstract PlaceActing TryStartAt( Place place );


    /// <summary>
    /// 检查活动是否已经完成
    /// </summary>
    /// <param name="acting">正在进行的活动对象</param>
    /// <returns>是否已经完成</returns>
    public abstract bool TryComplete( PlaceActing acting );

  }
}
