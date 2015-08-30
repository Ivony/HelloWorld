using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 建筑
  /// </summary>
  [Guid( "D681EF16-D0D2-4B72-834B-5ADDB0867535" )]
  public class BuildingDescriptor : GameRuleDataItem
  {


    private BuildingDescriptor( Guid guid, JObject data ) : base( guid, data ) { }

    public static BuildingDescriptor FromData( Guid guid, JObject data )
    {
      if ( data == null )
        return null;

      return new BuildingDescriptor( guid, data )
      {
        Name = data.Value<string>( "Name" ),
        Description = data.Value<string>( "Description" ),
      };
    }


    /// <summary>
    /// 建筑名
    /// </summary>
    public string Name
    {
      get; private set;
    }

    /// <summary>
    /// 建筑描述
    /// </summary>
    public string Description
    {
      get; private set;
    }
  }
}