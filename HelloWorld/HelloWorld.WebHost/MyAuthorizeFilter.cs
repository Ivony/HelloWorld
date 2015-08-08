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

    public override async Task OnAuthorizationAsync( HttpActionContext actionContext, CancellationToken cancellationToken )
    {
    }
  }
}