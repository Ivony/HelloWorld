using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 提供针对 JSON 的扩展方法。
  /// </summary>
  public static class JValueExtensions
  {


    /// <summary>
    /// 获取 GUID 类型的值
    /// </summary>
    public static Guid? GuidValue( this JObject obj, string name )
    {
      var value = obj[name] as JValue;
      return GuidValue( value );
    }


    /// <summary>
    /// 获取 GUID 类型的值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid? GuidValue( this JValue value )
    {
      if ( value == null || value.Type == JTokenType.Null )
        return null;


      if ( value.Type == JTokenType.Guid )
        return value.Value<Guid>();

      else if ( value.Type == JTokenType.String )
        return Guid.Parse( value.Value<string>() );

      else
        throw new InvalidCastException();
    }




    /// <summary>
    /// 获取 TimeSpan 类型的值
    /// </summary>
    public static TimeSpan TimeValue( this JObject obj, string name )
    {
      return TimeSpan.Parse( obj.Value<string>( name ) );
    }


    /// <summary>
    /// 获取 TimeSpan 类型的值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TimeSpan TimeValue( this JValue value )
    {
      return TimeSpan.Parse( value.Value<string>() );
    }





    /// <summary>
    /// 获取坐标值
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Coordinate CoordinateValue( this JObject obj, string name )
    {
      return Coordinate.Parse( obj.Value<string>( name ) );
    }


    /// <summary>
    /// 获取坐标值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Coordinate CoordinateValue( this JValue value )
    {
      return Coordinate.Parse( value.Value<string>() );
    }


  }
}
