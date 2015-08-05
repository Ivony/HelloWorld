using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace HelloWorld.WebHost
{
  public class Host
  {
    public static UserService UserService { get; private set; }
    public static GameDataService DataService { get; private set; }

    public static void Initailze( HttpConfiguration configuration )
    {

      var dataRoot = Path.Combine( HostingEnvironment.ApplicationPhysicalPath, "Data" );

      UserService = new JsonUserService( dataRoot );
      DataService = new JsonDataService( dataRoot );

      configuration.Services.Replace( typeof( IContentNegotiator ), new JsonContentNegotiator() );

    }


  }
}