using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HelloWorld.WebHost
{
  public class MyAuthorizeFilter : AuthorizationFilterAttribute
  {
    internal const string CookieName = "hu";

    public override async Task OnAuthorizationAsync( HttpActionContext actionContext, CancellationToken cancellationToken )
    {

      if ( actionContext.ControllerContext.ControllerDescriptor.ControllerType == typeof( UserController ) || string.Equals( actionContext.ActionDescriptor.ActionName, "Test", StringComparison.OrdinalIgnoreCase ) )
        return;

      var request = actionContext.Request;

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
        actionContext.Response = request.CreateErrorResponse( HttpStatusCode.Unauthorized, "Unauthorized" );

      else
        actionContext.Request.Properties["UserID"] = userId;
    }
  }
}