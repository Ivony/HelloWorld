using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 代表一个地块限制
  /// </summary>
  public sealed class PlaceRestriction
  {


    private PlaceRestriction() { }



    /// <summary>
    /// 从 JSON 数据中还原地块限制对象
    /// </summary>
    /// <param name="rules">游戏规则引擎</param>
    /// <param name="data">数据</param>
    /// <returns>地块限制对象</returns>
    public static PlaceRestriction FromData( GameRulesBase rules, JToken data )
    {
      var value = data as JValue;
      if ( value != null )
      {
        var building = rules.GetDataItem<BuildingDescriptor>( value.GuidValue() );
        return new PlaceRestriction
        {
          Building = building,
        };
      }

      else
        return null;

    }

    /// <summary>
    /// 建筑描述
    /// </summary>
    public BuildingDescriptor Building { get; set; }

    /// <summary>
    /// 建筑限制数据
    /// </summary>
    public object RestrictData { get; set; }

  }
}
