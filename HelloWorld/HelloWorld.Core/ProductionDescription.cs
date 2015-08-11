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
        RequiredTime = data.TimeValue( "RequiredTime" ),
      };
    }


    /// <summary>
    /// 此生产过程所依赖的建筑/场所
    /// </summary>
    public BuildingDescriptor Building { get; private set; }


    /// <summary>
    /// 此生产过程所需的时间
    /// </summary>
    public TimeSpan RequiredTime { get; private set; }


    /// <summary>
    /// 此生产过程所需的劳工数量
    /// </summary>
    public int Workers { get; private set; }

  }
}
