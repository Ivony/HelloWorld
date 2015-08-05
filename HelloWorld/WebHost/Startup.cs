using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using HelloWorld.Core;
using System.IO;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Authentication.Cookies;

namespace WebHost
{
  public class Startup
  {


    public static UserService UserService { get; private set; }
    public static GameDataService DataService { get; private set; }

    public Startup( IHostingEnvironment env )
    {

      UserService = new JsonUserService( Path.Combine( env.WebRootPath, "Data" ) );
      DataService = new JsonDataService( Path.Combine( env.WebRootPath, "Data" ) );
    }

    // This method gets called by a runtime.
    // Use this method to add services to the container
    public void ConfigureServices( IServiceCollection services )
    {
      services.AddMvc();
      // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
      // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
      // services.AddWebApiConventions();
      services.AddRouting();
    }

    // Configure is called after ConfigureServices is called.
    public void Configure( IApplicationBuilder app, IHostingEnvironment env )
    {
      // Configure the HTTP request pipeline.
      app.UseStaticFiles();

      // Add MVC to the request pipeline.
      app.UseMvc( routes => routes
        .MapRoute( "Game", "{action}", new { controller = "Game" } )
        .MapRoute( "Default", "{controller}/{action}" )
      );
      // Add the following route for porting Web API 2 controllers.
      // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
    }
  }
}
