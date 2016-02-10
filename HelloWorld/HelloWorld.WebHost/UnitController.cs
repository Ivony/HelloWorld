using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HelloWorld.WebHost
{
  public class UnitController : ControllerBase
  {


    /// <summary>
    /// 获取自己所有单位
    /// </summary>
    /// <returns></returns>
    public async Task<object> Get()
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
    public async Task<object> Get( Guid id )
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
    public async Task<object> Move( Guid id, string direction )
    {

      await Task.Yield();

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


  }
}