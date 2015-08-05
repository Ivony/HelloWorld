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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebHost.Controllers
{
  [MyAuthorizeFilter]
  public class GameController : ApiController
  {
    [HttpGet]
    public async Task<object> Test()
    {
      return "Test OK!";
    }



    [HttpGet]
    public Task<object> Info()
    {
      return null;
    }


  }
}
