using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 描述一个单位需求
  /// </summary>
  public class UnitRestriction
  {

    /// <summary>
    /// 单位描述
    /// </summary>
    public UnitDescriptor Unit { get; private set; }

    /// <summary>
    /// 单位限制
    /// </summary>
    public object Restrict { get; private set; }

    public static UnitRestriction FromData( GameRules rules, JToken data )
    {
      var value = data as JValue;
      if ( value != null )
      {
        var unit = rules.GetDataItem<UnitDescriptor>( value.GuidValue() );
        return new UnitRestriction
        {
          Unit = unit,
        };
      }

      else
        return null;

    }


  }
}
