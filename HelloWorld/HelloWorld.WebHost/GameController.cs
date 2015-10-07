using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;


namespace HelloWorld.WebHost
{
  public class GameController : ControllerBase
  {


    [HttpGet]
    public async Task<object> Test()
    {
      return "Test OK!";
    }



    [HttpGet]
    public object Info()
    {
      return Player.GetInfo();
    }



    [HttpGet]
    public async Task<object> Info( string nickname )
    {

      Player.Nickname = nickname;
      await Player.Save();

      return Info();
    }



    [HttpGet]
    public async Task<object> Place( string coordinate )
    {
      var place = await GetPlace( coordinate );

      if ( place == null )
        return null;

      else
        return place.GetInfo( Player );
    }

    private async Task<Place> GetPlace( string coordinate )
    {
      var place = Player.GetPlace( Coordinate.Parse( coordinate ) );
      if ( place.Owner != Player.UserID )
        return null;


      place.Check();

      return place;
    }



    [HttpGet]
    public async Task<object> Acting( string coordinate, Guid id )
    {

      var place = await GetPlace( coordinate );
      if ( place == null || place.Owner != Player.UserID )
        throw new InvalidOperationException( "不能在不是自己的领土上进行活动" );




      var gameItem = GameHost.GameRules.GetDataItem<ActionDescriptorBase>( id );
      var acting = gameItem.TryStartAt( place );

      if ( acting != null )
        return new
        {
          Success = true,
          Acting = acting.GetInfo(),
        };

      else
        return new
        {
          Success = false,
        };
    }




    public async Task<object> Info( string oldPassword, string newPassword )
    {
      return Host.UserService.TryResetPassword( LoginToken, oldPassword, newPassword );
    }



    [HttpGet]
    public async Task<object> Messages( DateTime? start = null, int count = 100 )
    {


      return Host.MessageService.GetMessages( Player.UserID, start ).Take( count ).ToArray();



    }


  }
}
