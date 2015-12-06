using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{


  /// <summary>
  /// 只能由特定单位执行的操作
  /// </summary>
  public class UnitActionDescriptor : ActionDescriptor
  {


    /// <summary>
    /// 单位限制
    /// </summary>
    public new UnitRestriction UnitRestriction
    {
      get; private set;
    }



    /// <summary>
    /// 用指定数据初始化对象
    /// </summary>
    /// <param name="data">数据</param>
    protected override void Initialize( JObject data )
    {
      base.Initialize( data );

      UnitRestriction = UnitRestriction.FromData( GameHost.GameRules, data["UnitRestriction"] );
    }


  }
}
