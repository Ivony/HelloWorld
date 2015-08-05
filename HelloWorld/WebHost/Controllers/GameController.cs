using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HelloWorld.Core;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebHost.Controllers
{
  public class GameController : Controller
  {

    public async Task<object> Test()
    {
      return "Test OK!";
    }



    public async Task<object> User( string email, string password )
    {
      Context.Response.Cookies.Delete( "u" );

      string loginToken;
      if ( Startup.UserService.TryLogin( email, password, out loginToken ) )
      {

        Context.Response.Cookies.Append( "u", loginToken );
        return new
        {
          Success = true,
          Mode = "Login",
          Email = email,
        };
      }

      else if ( Startup.UserService.TryRegister( email, password, out loginToken ) )
      {
        Context.Response.Cookies.Append( "u", loginToken );

        return new
        {
          Success = true,
          Mode = "Register",
          Email = email,
        };
      }
      else
      {
        return new
        {
          Success = false
        };
      }
    }

    private ClaimsPrincipal CreatePrincipal( Guid userId )
    {

      return new ClaimsPrincipal( new GenericIdentity( userId.ToString(), "HelloWorld" ) );
    }

  }
}
