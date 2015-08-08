using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using HelloWorld.WebHost;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using Newtonsoft.Json.Linq;
using System.Net;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWorld.WebHost
{
  [MyAuthorizeFilter]
  public class GameController : ApiController
  {


    internal const string CookieName = "hu";

    public async override Task<HttpResponseMessage> ExecuteAsync( HttpControllerContext controllerContext, CancellationToken cancellationToken )
    {
      var request = controllerContext.Request;

      string loginToken = null;

      if ( loginToken == null )
      {
        var authorization = request.Headers.Authorization;

        if ( authorization != null && authorization.Scheme == "Hello" )
          loginToken = authorization.Parameter;
      }

      if ( loginToken == null )
      {
        var cookie = request.Headers.GetCookies().SelectMany( c => c.Cookies ).Where( c => c.Name == CookieName ).FirstOrDefault();
        if ( cookie != null )
          loginToken = cookie.Value;

      }


      var userId = Host.UserService.GetUserID( loginToken );
      if ( userId == null )
        return request.CreateErrorResponse( HttpStatusCode.Unauthorized, "Unauthorized" );

      else
        Player = await Host.DataService.GetPlayer( userId.Value );

      return await base.ExecuteAsync( controllerContext, cancellationToken );
    }


    public Player Player { get; private set; }



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



    public async Task<object> Info( string nickname )
    {

      Player.Nickname = nickname;
      await Player.Save();

      return Info();
    }

  }
}
