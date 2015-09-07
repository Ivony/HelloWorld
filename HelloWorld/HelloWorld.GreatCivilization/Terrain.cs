using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld.GreatCivilization
{


  /// <summary>
  /// 定义基本地形
  /// </summary>
  public class Terrain : BuildingDescriptor
  {


    private Terrain( BuildingDescriptor building ) : base( building ) { }



    private static readonly Guid wildernessId = new Guid( "EB0C8AE8-FC09-4874-9985-98C081F4D1B7" );
    private static readonly Guid forestId = new Guid( "8105A10F-1E11-4FF0-B2D2-1FD910258648" );
    private static readonly Guid grasslandId = new Guid( "C6E4329A-D722-439C-B846-F0EADF388AD9" );
    private static readonly Guid poolId = new Guid( "EC5B8863-0A10-4D29-A238-B9A26C705765" );
    private static readonly Guid marshId = new Guid( "6EC01992-1693-4F56-B8DE-30BD0DD355FB" );


    public override void Check( Place place )
    {

      if ( place.Acting != null )//当地块上正在进行某种活动时，不进行地块转换。
        return;


      if ( Guid == forestId )
      {
        place.CheckPoint = DateTime.UtcNow;
        return;
      }


      var times = (int) ( DateTime.UtcNow - place.CheckPoint ).TotalMinutes / 5;//每五分钟一次改变的机会
      for ( int i = 0; i < times; i++ )
      {
        if ( TerrainChanged( place ) )
        {
          place.CheckPoint = place.CheckPoint.AddMinutes( i + 1 );
          place.Check();
          return;
        }
      }

      place.CheckPoint = place.CheckPoint.AddMinutes( times );
        
    }

    private bool TerrainChanged( Place place )
    {

      if ( Guid == wildernessId )
        return Probability.IfHit( 2d / 100d, () => place.Building = GameHost.GameRules.GetDataItem<BuildingDescriptor>( grasslandId ) )
          || Probability.IfHit( 1d / 1000d, () => place.Building = GameHost.GameRules.GetDataItem<BuildingDescriptor>( poolId ) );

      else if ( Guid == grasslandId )
        return Probability.IfHit( 1d / 100d, () => place.Building = GameHost.GameRules.GetDataItem<BuildingDescriptor>( forestId ) );

      else if ( Guid == poolId )
        return Probability.IfHit( 1d / 3000d, () => place.Building = GameHost.GameRules.GetDataItem<BuildingDescriptor>( marshId ) );

      else if ( Guid == marshId )
        return Probability.IfHit( 5d / 1000d, () => place.Building = GameHost.GameRules.GetDataItem<BuildingDescriptor>( wildernessId ) );

      else
        return false;
    }


    public new static Terrain FromData( Guid guid, JObject data )
    {

      var building = BuildingDescriptor.FromData( guid, data );

      if ( building == null )
        return null;

      return new Terrain( building );
    }



  }
}
