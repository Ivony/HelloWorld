﻿using System;
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


    protected override void Initialize( GameRulesBase rules, JObject data )
    {
      base.Initialize( rules, data );

      dynamic d = data;

      Name = d.Name;
      Description = d.Description;
      MobilityMaximum = d.Mobility.Maximum;
      MobilityRecoveryCycle = d.Mobility.RecoveryCycle;
      MobilityRecoveryScale = d.Mobility.RecoveryScale;

      InstanceType = rules.GetType( (string) d.InstanceType, typeof( Unit ) );
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
    /// 单位类型
    /// </summary>
    public Type InstanceType { get; private set; }


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
    /// 获取单位可以执行的操作列表
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public virtual ActionDescriptor[] GetActions( Unit unit )
    {
      return new ActionDescriptor[0];
    }


    private static object _sync = new object();
  }
}
