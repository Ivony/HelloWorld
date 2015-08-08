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


namespace HelloWorld.WebHost
{
  [MyAuthorizeFilter]
  public class GameController : ApiController
  {


    internal const string CookieName = "hu";

    public async override Task<HttpResponseMessage> ExecuteAsync( HttpControllerContext controllerContext, CancellationToken cancellationToken )
    {

      await Authentication( controllerContext.Request );
      return await base.ExecuteAsync( controllerContext, cancellationToken );

    }

    private async Task Authentication( HttpRequestMessage request )
    {
      if ( LoginToken == null )
      {
        var authorization = request.Headers.Authorization;

        if ( authorization != null && authorization.Scheme == "Hello" )
          LoginToken = authorization.Parameter;
      }

      if ( LoginToken == null )
      {
        var cookie = request.Headers.GetCookies().SelectMany( c => c.Cookies ).Where( c => c.Name == CookieName ).FirstOrDefault();
        if ( cookie != null )
          LoginToken = cookie.Value;

      }


      var userId = Host.UserService.GetUserID( LoginToken );
      if ( userId == null )
        throw new HttpResponseException( request.CreateErrorResponse( HttpStatusCode.Unauthorized, "Unauthorized" ) );

      Player = await Host.DataService.GetPlayer( userId.Value );
    }


    protected string LoginToken { get; private set; }

    protected Player Player { get; private set; }



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


    public async Task<object> Info( string oldPassword, string newPassword )
    {
      return Host.UserService.TryResetPassword( LoginToken, oldPassword, newPassword );
    }


  }
}
