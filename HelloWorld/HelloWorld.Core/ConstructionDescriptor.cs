using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  /// <summary>
  /// 定义一个建造过程描述
  /// </summary>
  public class ConstructionDescriptor : GameItemData
  {


    private ConstructionDescriptor( Guid guid, JObject data ) : base( guid, data ) { }


    internal static ConstructionDescriptor FromData( Guid guid, JObject data )
    {

      if ( data == null )
        return null;

      return new ConstructionDescriptor( guid, data )
      {
        OriginBuilding = GameEnvironment.GetBuilding( data.Value<Guid>( "OriginBuilding" ) ),
        NewBuiding = GameEnvironment.GetBuilding( data.Value<Guid>( "NewBuiding" ) ),
      };
    }



    /// <summary>
    /// 原建筑
    /// </summary>
    public BuildingDescriptor OriginBuilding { get; private set; }


    /// <summary>
    /// 新建筑
    /// </summary>
    public BuildingDescriptor NewBuiding { get; private set; }


    /// <summary>
    /// 生产过程所需资源描述
    /// </summary>
    public ResourceRequirment ResourceRequirment { get; private set; }

  }
}
