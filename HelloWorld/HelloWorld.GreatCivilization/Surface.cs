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
  public class Surface : Building
  {

    private static readonly Guid wildernessId = new Guid( "EB0C8AE8-FC09-4874-9985-98C081F4D1B7" );
    private static readonly Guid forestId = new Guid( "8105A10F-1E11-4FF0-B2D2-1FD910258648" );
    private static readonly Guid grasslandId = new Guid( "C6E4329A-D722-439C-B846-F0EADF388AD9" );
    private static readonly Guid poolId = new Guid( "EC5B8863-0A10-4D29-A238-B9A26C705765" );
    private static readonly Guid marshId = new Guid( "6EC01992-1693-4F56-B8DE-30BD0DD355FB" );


    public override void Check( DateTime now )
    {

      if ( Place.Acting != null )//当地块上正在进行某种活动时，不进行地块转换。
        return;


      if ( Descriptor.Guid == forestId )
      {
        Place.CheckPoint = now;
        return;
      }


      var times = (int) ( now - Place.CheckPoint ).TotalMinutes / 5;//每五分钟一次改变的机会
      for ( int i = 0; i < times; i++ )
      {
        if ( TerrainChanged( Place ) )
        {
          Place.CheckPoint = Place.CheckPoint.AddMinutes( i + 1 );
          Place.Check( now );
          return;
        }
      }

      Place.CheckPoint = Place.CheckPoint.AddMinutes( times * 5 );

    }

    private bool TerrainChanged( Place place )
    {

      if ( Descriptor.Guid == wildernessId )
        return Probability.IfHit( 2d / 100d, () => place.SetBuilding( grasslandId ) )
          || Probability.IfHit( 1d / 1000d, () => place.SetBuilding( poolId ) );

      else if ( Descriptor.Guid == grasslandId )
        return Probability.IfHit( 1d / 100d, () => place.SetBuilding( forestId ) );

      else if ( Descriptor.Guid == poolId )
        return Probability.IfHit( 1d / 3000d, () => place.SetBuilding( marshId ) );

      else if ( Descriptor.Guid == marshId )
        return Probability.IfHit( 5d / 1000d, () => place.SetBuilding( wildernessId ) );

      else
        return false;
    }

  }
}
