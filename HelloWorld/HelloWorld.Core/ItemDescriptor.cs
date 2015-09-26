using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{


  /// <summary>
  /// 定义一个物品描述信息
  /// </summary>
  [Guid( "F2C8BA6C-EB73-4787-8A3D-7C7EF6BF5E23" )]
  public class ItemDescriptor : GameRuleDataItem
  {


    protected override void Initialize( JObject data )
    {
      base.Initialize( data );

      Name = data.Value<string>( "Name" );
      Description = data.Value<string>( "Description" );
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
    internal string Expression { get { return string.Format( "{0:B}/{1}", Guid, Name ); } }

  }
}
