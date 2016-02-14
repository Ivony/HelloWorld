using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld.Starve
{
  public class StarveGameRules : GameRules
  {

    private StarveGameRules( IJsonDataResolver dataResolver, ITypeResolver typeResolver ) : base( dataResolver, typeResolver ) { }


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


    public static StarveGameRules Instance { get; private set; }

    internal ActionDescriptor[] GetActions()
    {
      return base.AllActions();

    }

    private static readonly object _sync = new object();

    public static StarveGameRules CreateInstance( ITypeResolver typeResolver )
    {
      return new StarveGameRules( new ResourceDataResolver( Assembly.GetExecutingAssembly() ), typeResolver );
    }





    public override ActionConstraint CreateConstraint( JObject data )
    {
      return new ActionConstraint( this, data ?? new JObject() );
    }


    /// <summary>
    /// self 单位 ID
    /// </summary>
    private static readonly Guid self = new Guid( "{72213162-0D16-4C53-89F3-AE2A0180E031}" );


    public override void InitializePlayer( GamePlayer player )
    {

      player.GetPlace( Coordinate.Origin ).NewUnit( self, player.Guid );

      base.InitializePlayer( player );

    }




    public override Place CreatePlace( Coordinate coordinate )
    {

      return new StarvePlace( coordinate );

    }



  }
}
