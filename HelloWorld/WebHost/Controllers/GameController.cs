using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HelloWorld.Core;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebHost.Controllers
{
  public class GameController : Controller
  {

    public async Task<object> Test()
    {
      return "Test OK!";
    }


    public async Task<object> Register( string email, string password )
    {

      Guid userId;
      if ( Startup.UserService.TryRegister( email, password, out userId ) )
        return new
        {
          Success = true,
          UserID = userId,
        };
      else
        return new
        {
          Success = false,
        };
    }



  }
}
