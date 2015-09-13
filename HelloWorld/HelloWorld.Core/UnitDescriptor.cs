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


    protected UnitDescriptor( UnitDescriptor instance ) : this( instance.Guid, instance.Data )
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



    internal static ActionDescriptor[] GetActions( Unit unit )
    {
      throw new NotImplementedException();
    }
  }
}
