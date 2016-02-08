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
      await Task.Yield();
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
      await Task.Yield();
      var place = Player.GetPlace( Coordinate.Parse( coordinate ) );
      if ( place.Owner != Player.UserID )
        return null;


      place.Check();

      return place;
    }


    /// <summary>
    /// 获取自己所有单位
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<object> Unit()
    {

      await Task.Yield();


      var units = GameHost.DataService.GetUnits( Player );
      return units.Select( item => new { item.Guid, item.Name } );

    }


    /// <summary>
    /// 获取单位信息
    /// </summary>
    /// <param name="id">单位ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<object> Unit( Guid id )
    {
      await Task.Yield();

      Unit unit = GetUnit( id );
      if ( unit == null )
        return NotFound();

      return unit.GetInfo();
    }

    private Unit GetUnit( Guid id )
    {
      return GameHost.DataService.GetUnits( Player ).FirstOrDefault( item => item.Guid == id );
    }


    /// <summary>
    /// 移动单位
    /// </summary>
    /// <param name="move">移动方向</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<object> Unit( Guid id, string direction )
    {

      var unit = GetUnit( id );
      if ( unit == null )
        return NotFound();




      Direction dir;

      switch ( direction.ToUpperInvariant() )
      {

        case "E":
          dir = Direction.East;
          break;

        case "W":
          dir = Direction.West;
          break;

        case "SE":
          dir = Direction.SouthEast;
          break;

        case "NE":
          dir = Direction.NorthEast;
          break;

        case "SW":
          dir = Direction.SouthWest;
          break;

        case "NW":
          dir = Direction.NorthWest;
          break;

        default:
          throw new Exception( "invalid direction" );
      }


      var success = unit.Move( dir );

      return new
      {
        Success = success,
        Unit = unit.GetInfo(),
      };
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
      await Task.Yield();
      return Host.UserService.TryResetPassword( LoginToken, oldPassword, newPassword );
    }



    [HttpGet]
    public async Task<object> Messages( DateTime? start = null, int count = 100 )
    {
      await Task.Yield();

      return Host.MessageService.GetMessages( Player.UserID, start ).Take( count ).ToArray();
    }

  }
}
