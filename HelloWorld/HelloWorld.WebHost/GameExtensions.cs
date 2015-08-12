using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace HelloWorld.WebHost
{
  public static class GameExtensions
  {

    public async static Task<GamePlayer> GetCurrentPlayer( this HttpControllerContext context )
    {
      return await GetCurrentPlayer( context.Request );
    }

    private async static Task<GamePlayer> GetCurrentPlayer( this HttpRequestMessage request )
    {
      return await Host.DataService.GetPlayer( (Guid) request.Properties["UserID"] );
    }
  }
}