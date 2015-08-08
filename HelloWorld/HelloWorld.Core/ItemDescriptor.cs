using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{


  /// <summary>
  /// 定义一个物品描述信息
  /// </summary>
  public class ItemDescriptor : GameItemData
  {


    private ItemDescriptor( Guid guid, JObject data ) : base( guid, data ) { }


    internal static ItemDescriptor FromData( Guid guid, JObject data )
    {

      if ( data == null )
        return null;

      return new ItemDescriptor( guid, data )
      {
        Name = data.Value<string>( "Name" ),
        Description = data.Value<string>( "Description" ),
      };
    }


    /// <summary>
    /// 物品名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 物品描述
    /// </summary>
    public string Description { get; private set; }


    /// <summary>
    /// 获取表达形式
    /// </summary>
    internal string Expression { get { return string.Format( "{0}({1})", Name, Guid ); } }

  }
}
