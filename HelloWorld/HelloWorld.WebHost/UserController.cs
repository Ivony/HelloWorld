using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HelloWorld.WebHost
{
  public class UserController : ApiController
  {
    public async Task<object> Get( string email, string password )
    {
      string loginToken;


      if ( Host.UserService.TryLogin( email, password, out loginToken ) )
      {
        return new
        {
          Success = true,
          Mode = "Login",
          LoginToken = loginToken,
          Email = email,
        };
      }

      else if ( Host.UserService.TryRegister( email, password, out loginToken ) )
      {
        return new
        {
          Success = true,
          Mode = "Register",
          LoginToken = loginToken,
          Email = email,
        };
      }
      else
      {
        return new
        {
          Success = false,
          Reason = "secret",
        };
      }
    }
  }
}