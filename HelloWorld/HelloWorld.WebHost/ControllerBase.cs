using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace HelloWorld.WebHost
{
  public class ControllerBase : ApiController
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




  }
}