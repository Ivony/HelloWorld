using Newtonsoft.Json.Linq;
using System;

namespace HelloWorld
{

  /// <summary>
  /// 建筑
  /// </summary>
  public class BuildingDescriptor : GameItemData
  {


    private BuildingDescriptor( Guid guid, JObject data ) : base( guid, data ) { }

    internal BuildingDescriptor Create( Guid guid, JObject data )
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