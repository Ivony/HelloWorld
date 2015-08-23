using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  /// <summary>
  /// 定义一个建造过程描述
  /// </summary>
  [Guid( "019C6290-1E13-4757-985A-F00C47EF5787" )]
  public class ConstructionDescriptor : GameActingDescriptor
  {


    private ConstructionDescriptor( Guid guid, JObject data ) : base( guid, data ) { }


    public static ConstructionDescriptor FromData( Guid guid, JObject data )
    {

      if ( data == null )
        return null;

      return new ConstructionDescriptor( guid, data )
      {
        RawBuilding = GameEnvironment.GetDataItem<BuildingDescriptor>( data.GuidValue( "RawBuilding" ) ),
        NewBuiding = GameEnvironment.GetDataItem<BuildingDescriptor>( data.GuidValue( "NewBuilding" ) ),
        Requirment = GameActingInvestmentDescriptor.FromData( (JObject) data["Input"] ),
      };
    }

    /// <summary>
    /// 原建筑
    /// </summary>
    public BuildingDescriptor RawBuilding { get; private set; }


    /// <summary>
    /// 新建筑
    /// </summary>
    public BuildingDescriptor NewBuiding { get; private set; }



    /// <summary>
    /// 生产过程所投入的资源描述
    /// </summary>
    public GameActingInvestmentDescriptor Requirment { get; set; }



    /// <summary>
    /// 尝试在指定地块开始这个活动 
    /// </summary>
    /// <param name="place">要开始活动的地方</param>
    /// <returns>正在进行的活动</returns>
    public override GameActing TryStartAt( Place place )
    {

      var acting = new GameActing( this );

      lock ( place )
      {
        if ( place.Acting != null )
          throw new InvalidOperationException();

        acting.StartAt( place );
      }

      return acting;
    }

    /// <summary>
    /// 检查活动是否已经完成，若已经完成则获取相应的收益
    /// </summary>
    /// <param name="acting">正在进行的活动</param>
    /// <returns>活动是否已经完成</returns>
    public override bool TryComplete( GameActing acting )
    {

      if ( acting.StartOn + Requirment.Time > DateTime.UtcNow )
        return false;

      acting.Place.Owner.Workers += Requirment.Workers;
      acting.Place.Building = NewBuiding;
      return true;
    }
  }
}
