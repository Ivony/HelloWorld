using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HelloWorld.Core
{
  public class JsonDataHost : GameDataService
  {

    public JsonDataHost( string dataRoot )
    {
      DataRoot = dataRoot;
      Directory.CreateDirectory( DataRoot );
      Directory.CreateDirectory( Path.Combine( DataRoot, _places ) );
      Directory.CreateDirectory( Path.Combine( DataRoot, _players ) );
    }


    private const string _places = "Places";
    private const string _players = "Players";
    private const string _extensions = ".json";


    public string DataRoot
    {
      get; private set;
    }

    public override Task<Place> GetPlace( Coordinate coordinate )
    {
    }


    private abstract class JsonDataItem
    {


      private void Initialize( string filepath )
      {
        FilePath = filepath;

        var data = File.ReadAllText( filepath );
        JsonConvert.PopulateObject( data, this );
      }


      protected string FilePath
      {
        get; private set;
      }


      private JsonSerializer serializer = JsonSerializer.CreateDefault();

      protected void Save()
      {
        using ( var writer = new StreamWriter( FilePath, false, Encoding.UTF8 ) )
        {
          serializer.Serialize( writer, this );
        }
      }



      public static T LoadData<T>( string path, string defaultValue ) where T : JsonDataItem, new()
      {

        if ( File.Exists( path ) == false )
        {
          if ( defaultValue == null )
            return null;


          File.WriteAllText( path, defaultValue );
        }


        var result = new T();
        result.Initialize( path );

        return result;


      }



    }


    private class JsonPlayer : JsonDataItem, IPlayer
    {
      public string Email
      {
        get;
        set;
      }
    }


    public override async Task<IPlayer> GetPlayer( Guid userId )
    {
      var filepath = Path.ChangeExtension( Path.Combine( DataRoot, _players, userId.ToString() ), _extensions );

      if ( File.Exists( filepath ) == false )
        return null;


      string data;
      using ( var reader = File.OpenText( filepath ) )
      {
        data = await reader.ReadToEndAsync();
      }

      return JsonConvert.DeserializeObject<IPlayer>( data );

    }
  }
}
