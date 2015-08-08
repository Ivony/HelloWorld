using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HelloWorld.WebHost
{
  public class UserController : ControllerBase
  {

    

    public async Task<object> Get( string email, string password, bool writeCookie = false )
    {
      string loginToken;
      string mode;


      if ( Host.UserService.TryLogin( email, password, out loginToken ) )
        mode = "Login";

      else if ( Host.UserService.TryRegister( email, password, out loginToken ) )
        mode = "Register";

      else
        mode = null;

      if ( mode == null )
        return new
        {
          Success = false,
          Reason = "Unknow"
        };

      var result = new { Success = true, Mode = mode, LoginToken = loginToken };

      if ( writeCookie )
      {
        var response = Request.CreateResponse( result );

        response.Headers.AddCookies( new[] { new CookieHeaderValue( GameController.CookieName, loginToken ) { Path = "/", Expires = DateTimeOffset.Now.AddDays( 1 ) } } );

        return response;
      }
      else
        return result;



    }
  }
}