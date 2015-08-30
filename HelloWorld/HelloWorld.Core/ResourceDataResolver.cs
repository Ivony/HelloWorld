using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  public class ResourceDataResolver : IJsonDataResolver
  {

    private Assembly[] _assemblies;

    public ResourceDataResolver( params Assembly[] assemblies )
    {
      _assemblies = assemblies;
    }


    public JObject LoadDataObject( string name )
    {

      var stream = _assemblies.Select( a => a.GetManifestResourceStream( name ) ).FirstOrDefault();
      return LoadData( stream );

    }

    public IEnumerable<JObject> LoadAllData()
    {
      foreach ( var stream in _assemblies.SelectMany( a => a.GetManifestResourceNames().OrderBy( name => name ).Select( name => a.GetManifestResourceStream( name ) ) ) )
      {
        yield return LoadData( stream );
      }
    }
    private static JObject LoadData( Stream stream )
    {
      using ( var reader = new StreamReader( stream ) )
      {
        return JObject.Parse( reader.ReadToEnd() );
      }
    }

  }
}
