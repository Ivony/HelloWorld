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

      player.GetPlace( Coordinate.Origin ).SetBuilding( GetDataItem<ImmovableDescriptor>( palace ) );
    }


    public override Place CreatePlace( Coordinate coordinate )
    {
      return new CivPlace( coordinate );
    }

  }
}
