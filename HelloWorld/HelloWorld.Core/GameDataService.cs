using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class GameDataService
  {
    public abstract Place GetPlace( Coordinate coordinate );

    public abstract GamePlayer GetPlayer( Guid userId );


    protected Coordinate GetInitiation()
    {
      for ( int i = 0; i < 100; i++ )
      {
        var initiation = Coordinate.RandomCoordinate( 1000, 1000 );

        if ( initiation.NearlyCoordinates( 6 ).Any( item => GetPlace( item ).Owner != null ) )
          continue;

        return initiation;
      }

      throw new InvalidOperationException( "无法找到合适的初始点" );
    }

  }
}
