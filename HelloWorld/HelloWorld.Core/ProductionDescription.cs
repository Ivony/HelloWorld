using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个生产过程
  /// </summary>
  public sealed class ProductionDescription : GameItemData
  {


    private ProductionDescription( Guid guid, JObject data ) : base( guid, data ) { }


    internal static ProductionDescription FromData( Guid guid, JObject data )
    {

      if ( data == null )
        return null;

      return new ProductionDescription( guid, data )
      {
        Building = GameEnvironment.GetBuilding( data.GuidValue( "Building" ) ),
        Requirment = ResourceRequirment.FromData( (JObject) data["Requirment"] ),
      };
    }


    /// <summary>
    /// 此生产过程所依赖的建筑/场所
    /// </summary>
    public BuildingDescriptor Building { get; private set; }



    /// <summary>
    /// 生产所需资源列表
    /// </summary>
    public ResourceRequirment Requirment { get; private set; }
  }
}
