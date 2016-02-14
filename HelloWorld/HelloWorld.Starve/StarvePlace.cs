using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Starve
{
  public class StarvePlace : Place
  {

    public StarvePlace( Coordinate coordinate ) : base( coordinate ) { }


    private Guid 平原 = new Guid( "{A70F06B8-341E-4DB4-9D8C-13E073CD2E8B}" );
    private Guid 丘陵 = new Guid( "{19C90055-B9E5-4EF0-9DD7-DCEDC338FCD5}" );
    private Guid 沙漠 = new Guid( "{E71C4DA2-0CFA-4142-BD9F-678D5F5B43CD}" );
    private Guid 戈壁 = new Guid( "{523BB3EF-EE89-4C1F-B51E-26B08C4D378B}" );



    protected override void Initialize()
    {
      base.Initialize();

      if ( Terrain == null )
      {

        var terrain = Probability
          .Map( 1000, 平原 )
          .Map( 500, 丘陵 )
          .Map( 100, 沙漠 )
          .Map( 150, 戈壁 )
          .Choose();

        SetTerrain( terrain );
      }


    }
  }
}
