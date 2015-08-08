using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public static class JValueExtensions
  {
    /// <summary>
    /// 获取 GUID 类型的值
    /// </summary>
    public static Guid GuidValue( this JObject obj, string name )
    {
      return Guid.Parse( obj.Value<string>( name ) );
    }


    /// <summary>
    /// 获取 GUID 类型的值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid GuidValue( this JValue value )
    {
      return Guid.Parse( value.Value<string>() );
    }
  }
}
