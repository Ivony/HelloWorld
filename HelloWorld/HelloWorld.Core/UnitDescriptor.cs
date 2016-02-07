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
  public class UnitDescriptor : GameRuleItem
  {


    protected override void Initialize( JObject data )
    {
      base.Initialize( data );

      dynamic d = data;

      Name = d.Name;
      Description = d.Description;
      MobilityMaximum = d.Mobility.Maximum;
      MobilityRecoveryCycle = d.Mobility.RecoveryCycle;
      MobilityRecoveryScale = d.Mobility.RecoveryScale;
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
    /// 最大行动力
    /// </summary>
    public decimal MobilityMaximum { get; private set; }



    /// <summary>
    /// 行动力恢复周期
    /// </summary>
    public TimeSpan MobilityRecoveryCycle { get; private set; }


    /// <summary>
    /// 每个恢复周期恢复的行动力
    /// </summary>
    public decimal MobilityRecoveryScale { get; private set; }




    /// <summary>
    /// 创建单位对象
    /// </summary>
    /// <param name="dataService">数据服务</param>
    /// <param name="owner">单位所属玩家</param>
    /// <param name="coordinate">单位所在坐标</param>
    /// <returns></returns>
    public Unit CreateUnit( IGameDataService dataService, Guid owner, Coordinate coordinate )
    {
      return new Unit( dataService, owner, dataService.NameService.AllocateName(), owner, this, coordinate );
    }



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

  }
}
