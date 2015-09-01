using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.GreatCivilization
{
  public class GreatCivilizationRules : GameRules
  {

    public GreatCivilizationRules( ITypeResolver typeResolver )
      : base( new ResourceDataResolver( Assembly.GetExecutingAssembly() ), typeResolver )
    {


    }


    public override void Initialize()
    {
      base.Initialize();

      _initiationBuilding = GetDataItem<BuildingDescriptor>( new Guid( "eb0c8ae8-fc09-4874-9985-98c081f4d1b7" ) );
    }

    BuildingDescriptor _initiationBuilding;

    public override BuildingDescriptor InitiationBuilding
    {
      get { return _initiationBuilding; }
    }

    /// <summary>
    /// 宫殿 ID
    /// </summary>
    private static readonly Guid palace = new Guid( "DE9E65D4-F49A-44A1-815C-66ED691D3FBF" );


    /// <summary>
    /// 重写初始化玩家对象方法，给玩家初始点放上一个宫殿。
    /// </summary>
    /// <param name="player"></param>
    public override void InitializePlayer( GamePlayer player )
    {
      base.InitializePlayer( player );

      player.GetPlace( Coordinate.Origin ).Building = GetDataItem<BuildingDescriptor>( palace );
    }

  }
}
