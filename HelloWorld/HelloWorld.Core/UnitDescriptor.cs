using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  /// <summary>
  /// 定义一个单位描述信息
  /// </summary>
  public class UnitDescriptor : GameRuleDataItem
  {
    private UnitDescriptor( Guid guid, JObject data ) : base( guid, data ) { }


    protected UnitDescriptor( UnitDescriptor instance )
      : this( instance.Guid, instance.Data )
    {
      Name = instance.Name;
      Description = instance.Description;
    }


    /// <summary>
    /// 单位名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 单位描述
    /// </summary>
    public string Description { get; private set; }


    /// <summary>
    /// 行动力
    /// </summary>
    public decimal Mobility { get; private set; }


    /// <summary>
    /// 完成任务后所需要的休息时间
    /// </summary>
    public TimeSpan RestTime { get; private set; }



    /// <summary>
    /// 获取单位可以执行的操作列表
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public virtual ActionDescriptor[] GetActions( Unit unit )
    {
      return new ActionDescriptor[0];
    }


    private static object _sync = new object();

    private HashSet<ActionDescriptor> _actions = new HashSet<ActionDescriptor>();

    /// <summary>
    /// 注册一个依赖于该单位的活动
    /// </summary>
    /// <param name="action"></param>
    internal void RegisterAction( ActionDescriptor action )
    {

      lock ( _sync )
      {
        _actions.Add( action );
      }

    }



    public Unit Create( Place place )
    {
      lock ( place.SyncRoot )
      {
        if ( place.Unit != null )
          throw new InvalidOperationException();

        return place.Unit = new Unit( Guid.NewGuid(), this, place );
      }
    }

  }
}
