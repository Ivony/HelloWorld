using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个生产过程
  /// </summary>
  [Guid( "AA393552-5020-4400-B565-FA613AF7A123" )]
  public sealed class ProductionDescriptor : GameActingDescriptor
  {


    private ProductionDescriptor( Guid guid, JObject data ) : base( guid, data ) { }


    internal static ProductionDescriptor FromData( Guid guid, JObject data )
    {

      if ( data == null )
        return null;

      return new ProductionDescriptor( guid, data )
      {
        Building = GameEnvironment.GetBuilding( data.GuidValue( "Building" ) ),
        Requirment = GameActingInvestmentDescriptor.FromData( (JObject) data["Requirment"] ),
        Returns = ItemList.FromData( (JArray) data["Returns"] ),
      };
    }


    /// <summary>
    /// 此生产过程所依赖的建筑/场所
    /// </summary>
    public BuildingDescriptor Building { get; private set; }



    /// <summary>
    /// 生产所需资源列表
    /// </summary>
    public GameActingInvestmentDescriptor Requirment { get; private set; }


    /// <summary>
    /// 生产结束后的回报
    /// </summary>
    public ItemList Returns { get; private set; }


    public override bool TryInvest( Place place )
    {
      throw new NotImplementedException();
    }

    public override void GetReturns( Place place )
    {
      place.Owner.Resources.AddItems( Returns );
    }
  }
}
