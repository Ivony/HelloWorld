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

    private sealed class JsonDataItem
    {


      private JsonDataItem( string filepath )
      {
        Filepath = filepath;

        var data = File.ReadAllText( filepath );
        Data = JObject.Parse( data );
      }

      public string Filepath
      {
        get; private set;
      }


      public dynamic Data
      {
        get; private set;
      }


      private JsonSerializer serializer = JsonSerializer.CreateDefault();

      public void Save()
      {
        File.WriteAllText( Filepath, ((JObject) Data).ToString( Formatting.None ) );
      }



      public static JsonDataItem LoadData( string path, string defaultValue )
      {

        if ( File.Exists( path ) == false )
        {
          if ( defaultValue == null )
            return null;


          File.WriteAllText( path, defaultValue );
        }


        return new JsonDataItem( path );

      }
    }



    private class JsonPlace : Place
    {

      private JsonDataItem _data;

      public JsonPlace( JsonDataItem data )
      {
        _data = data;
      }

      public override BuildingDescriptor Building
      {
        get { return GameEnvironment.GetBuilding( Guid.Parse( _data.Data.Building ) ); }

        set
        {
          _data.Data.Building = value.Guid.ToString();
        }
      }

      public override Coordinate Coordinate
      {
        get
        {
          throw new NotImplementedException();
        }
      }
    }




    private class JsonPlayer : Player
    {
      private JsonDataItem data;

      public JsonPlayer( JsonDataItem data )
      {
        this.data = data;
      }

      public override string Email
      {
        get { return data.Data.Email; }
      }
    }


    public override Task<Player> GetPlayer( Guid userId )
    {
      var filepath = Path.ChangeExtension( Path.Combine( DataRoot, _players, userId.ToString() ), _extensions );

      var data = JsonDataItem.LoadData( filepath, null );
      if ( data == null )
        return null;

      return Task.FromResult( (Player) new JsonPlayer( data ) );

    }


    public override Task<Place> GetPlace( Coordinate coordinate )
    {
      var filepath = Path.ChangeExtension( Path.Combine( DataRoot, _places, coordinate.ToString() ), _extensions );

      var data = JsonDataItem.LoadData( filepath, "{'building':'{EB0C8AE8-FC09-4874-9985-98C081F4D1B7}'}" );

      return Task.FromResult( (Place) new JsonPlace( data ) );

    }
  }
