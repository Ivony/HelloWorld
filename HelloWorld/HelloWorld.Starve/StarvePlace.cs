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
    private Guid 戈壁 = new Guid( "{523BB3EF-EE89-4C1F-B51E-26B08C4D378B}" );


    private Guid 苹果树 = new Guid( "{C6E4329A-D722-439C-B846-F0EADF388AD9}" );
    private Guid 梨树 = new Guid( "{883B0574-01FB-4118-83F1-130C16C8A5FE}" );
    private Guid 榕树 = new Guid( "{4FF27752-26B3-4FC3-83E6-E58D472824FE}" );
    private Guid 小麦 = new Guid( "{6ED6C430-E1A5-4E93-911E-B21738EAFC4F}" );
    private Guid 野花 = new Guid( "{990EE768-77E0-4106-A10A-4BE84F2E71C8}" );

    private Guid 石头 = new Guid( "{4740FDAC-C0C2-469D-9C2A-029DB81E7FF4}" );
    private Guid 铁矿 = new Guid( "{819574C8-F57E-4AFA-8FD2-4644022769D8}" );
    private Guid 青铜矿 = new Guid( "{10D946F8-1DBB-46D1-AF16-A70E69AA1329}" );




    private DateTime _lastBuildingDestoried;


    protected override void Initialize()
    {
      base.Initialize();

      _lastBuildingDestoried = (DateTime?) DataObject.LastBuildingDestoried ?? DateTime.MinValue;

      if ( Terrain == null )
      {

        var terrain = Probability
          .Map( 1000, 平原 )
          .Map( 500, 丘陵 )
          .Map( 150, 戈壁 )
          .Choose();

        SetTerrain( terrain );
      }

    }


    public override void Check( DateTime now )
    {
      base.Check( now );

      if ( Building == null && _lastBuildingDestoried < DateTime.UtcNow.AddMinutes( -10 ) )
      {
        if ( Terrain.Descriptor.Guid == 平原 )
        {
          var building = Probability
            .Map( 300, 野花 )
            .Map( 100, 榕树 )
            .Map( 50, 苹果树 )
            .Map( 30, 梨树 )
            .Map( 20, 小麦 )
            .Map( 20, 石头 )
            .Choose();

          SetBuilding( building );
        }

        else if ( Terrain.Descriptor.Guid == 丘陵 )
        {
          var building = Probability
            .Map( 800, 野花 )
            .Map( 500, 榕树 )
            .Map( 20, 梨树 )
            .Map( 1000, 石头 )
            .Map( 200, 铁矿 )
            .Map( 200, 青铜矿 )
            .Choose();

          SetBuilding( building );
        }

        else if ( Terrain.Descriptor.Guid == 戈壁 )
        {

          var building = Probability
            .Map( 1000, 石头 )
            .Map( 200, 铁矿 )
            .Map( 200, 青铜矿 )
            .Choose();

          SetBuilding( building );
        }
      }

    }

    public override void SetBuilding( Guid? id )  //UNDONE 增加时间参数
    {

      if ( Building != null && id == null )
        _lastBuildingDestoried = DataObject.LastBuildingDestoried = DateTime.UtcNow;

      base.SetBuilding( id );
    }


    public override object GetInfo( GamePlayer player )
    {
      if ( player.Units().All( item => item.Coordinate.Distance( Coordinate ) > 1 ) )
        return null;

      return base.GetInfo( player );
    }
  }
}
