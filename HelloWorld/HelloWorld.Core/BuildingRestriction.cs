using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  public sealed class BuildingRestriction
  {


    private BuildingRestriction() { }



    public static BuildingRestriction FromData( GameRules rules, JToken data )
    {
      var value = data as JValue;
      if ( value != null )
      {
        var building = rules.GetDataItem<BuildingDescriptor>( value.GuidValue() );
        return new BuildingRestriction
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
