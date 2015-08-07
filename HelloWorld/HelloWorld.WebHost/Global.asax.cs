﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace HelloWorld.WebHost
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start( object sender, EventArgs e )
    {

      RegisterRoutes( GlobalConfiguration.Configuration.Routes );

      Host.Initailze( GlobalConfiguration.Configuration );


    }

    private static void RegisterRoutes( HttpRouteCollection routes )
    {
      routes.MapHttpRoute( "Info", "", new { controller = "Game", action = "Info" } );
      routes.MapHttpRoute( "User", "User", new { controller = "User" } );
      routes.MapHttpRoute( "Game", "{action}", new { controller = "Game" } );
      routes.MapHttpRoute( "Default", "{controller}/{action}" );


    }
  }
}