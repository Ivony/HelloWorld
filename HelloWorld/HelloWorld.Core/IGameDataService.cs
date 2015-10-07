using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  public interface IGameDataService
  {
    Place GetPlace( Coordinate coordinate );

    GamePlayer GetPlayer( Guid userId );



    Unit[] GetUnits( Coordinate coordinate );

    Unit[] GetUnits( GamePlayer player );


    void Save( GameDataItem dataItem );
  }


}
