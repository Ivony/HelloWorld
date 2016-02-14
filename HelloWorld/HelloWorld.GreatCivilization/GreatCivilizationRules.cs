using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld.GreatCivilization
{
  public class GreatCivilizationRules : GameRules
  {

    private GreatCivilizationRules( IJsonDataResolver dataResolver, ITypeResolver typeResolver ) : base( dataResolver, typeResolver ) { }


    public override void Initialize()
    {
      lock ( _sync )
      {

        if ( Instance != null )
          throw new InvalidOperationException();

        base.Initialize();

        Instance = this;
      }

    }


    /// <summary>
    /// 宫殿 ID
    /// </summary>
    private static readonly Guid palace = new Guid( "DE9E65D4-F49A-44A1-815C-66ED691D3FBF" );


    public static GreatCivilizationRules Instance { get; private set; }

    internal ActionDescriptor[] GetActions()
    {
      return base.AllActions();

    }

    private static readonly object _sync = new object();

    public static GreatCivilizationRules CreateInstance( ITypeResolver typeResolver )
    {
      return new GreatCivilizationRules( new ResourceDataResolver( Assembly.GetExecutingAssembly() ), typeResolver );
    }



    /// <summary>
    /// 重写初始化玩家对象方法，给玩家初始点放上一个宫殿。
    /// </summary>
    /// <param name="player"></param>
    public override void InitializePlayer( GamePlayer player )
    {
      base.InitializePlayer( player );

      player.GetPlace( Coordinate.Origin ).SetBuilding( palace );
    }


    public override Place CreatePlace( Coordinate coordinate )
    {
      return new CivPlace( coordinate );
    }

    public override ActionConstraint CreateConstraint( JObject data )
    {
      return new ActionConstraint( this, data ?? new JObject() );
    }
  }
}
