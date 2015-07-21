using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{


  /// <summary>
  /// 定义一个物品描述信息
  /// </summary>
  public class ItemDescriptor
  {


    private ItemDescriptor() { }



    internal ItemDescriptor Create( JObject data )
    {

      if ( data == null )
        return null;

      return new ItemDescriptor
      {
        Name = data.Value<string>( "Name" ),
        Description = data.Value<string>( "Description" ),
        _json = data.ToString(),
      };
    }


    public string Name { get; private set; }

    public string Description { get; private set; }



    private string _json;

    public override string ToString() { return _json; }


  }
}
