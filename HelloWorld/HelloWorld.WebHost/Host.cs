using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using HelloWorld.GreatCivilization;

namespace HelloWorld.WebHost
{
  public class Host
  {
    public static IGameUserService UserService { get; private set; }
    public static IGameDataService DataService { get; private set; }

    public static void Initailze( HttpConfiguration configuration )
    {



      var configPath = ConfigurationManager.AppSettings["DataPath"] ?? "Data";
      var dataRoot = Path.Combine( HostingEnvironment.ApplicationPhysicalPath, configPath );

      UserService = new JsonUserService( dataRoot );
      DataService = new JsonDataService( dataRoot );


      var typeResolver = new HttpRuntimeTypeResolver( configuration.Services.GetAssembliesResolver() );


      var rules = new GreatCivilizationRules( typeResolver );
      GameHost.Initialize( rules, DataService );



      configuration.Services.Replace( typeof( IContentNegotiator ), new JsonContentNegotiator() );

    }


  }
}