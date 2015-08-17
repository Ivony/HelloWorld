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

    public static GamePlayer GetCurrentPlayer( this HttpControllerContext context )
    {
      return GetCurrentPlayer( context.Request );
    }

    private static GamePlayer GetCurrentPlayer( this HttpRequestMessage request )
    {
      return Host.DataService.GetPlayer( (Guid) request.Properties["UserID"] );
    }
  }
}