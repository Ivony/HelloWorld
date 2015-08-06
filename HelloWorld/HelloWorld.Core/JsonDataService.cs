using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace HelloWorld
{
  public class JsonDataService : GameDataService
  {

    public JsonDataService( string dataRoot )
    {
      DataRoot = dataRoot;
      Directory.CreateDirectory( DataRoot );
      Directory.CreateDirectory( placesDirectory = Path.Combine( DataRoot, "Places" ) );
      Directory.CreateDirectory( playersDirectory = Path.Combine( DataRoot, "Players" ) );
    }



    private readonly string placesDirectory;
    private readonly string playersDirectory;


    private const string _extensions = ".json";


    public string DataRoot
    {
      get; private set;
    }

    private sealed class JsonDataItem : JObject
    {


      private JsonDataItem( string filepath )
      {
        _filepath = filepath;

        var data = JObject.Parse( File.ReadAllText( filepath ) );
        this.Merge( data );
      }

      private string _filepath;
      private JsonSerializer serializer = JsonSerializer.CreateDefault();



      protected override void OnPropertyChanged( string propertyName )
      {
        Save();
      }



      private void Save()
      {
        File.WriteAllText( _filepath, ((JObject) this).ToString( Formatting.None ) );
      }



      /// <summary>
      /// 加载 JsonData 对象
      /// </summary>
      /// <param name="path">文件路径</param>
      /// <param name="defaultValue">默认值</param>
      /// <returns></returns>
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


      /// <summary>
      /// 加载 JsonData 对象
      /// </summary>
      /// <param name="path">文件路径</param>
      /// <param name="defaultValue">默认值</param>
      /// <returns></returns>
      public static JsonDataItem LoadData( string path, object defaultValue )
      {
        return LoadData( path, JObject.FromObject( defaultValue ).ToString() );
      }

    }



    private class JsonPlace : Place
    {

      private dynamic _data;

      public JsonPlace( JsonDataItem data )
      {
        _data = data;
      }

      public override BuildingDescriptor Building
      {
        get { return GameEnvironment.GetBuilding( Guid.Parse( _data.Building ) ); }

        set
        {
          _data.Building = value.Guid.ToString();
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
      private dynamic data;

      public JsonPlayer( JsonDataItem data )
      {
        this.data = data;
      }

      public override string NickName
      {
        get { return data.NickName; }
      }
    }


    public override Task<Player> GetPlayer( Guid userId )
    {
      var filepath = Path.ChangeExtension( Path.Combine( playersDirectory, userId.ToString() ), _extensions );

      var data = JsonDataItem.LoadData( filepath, "{ 'NickName': 'NoBody' }" );

      return Task.FromResult( (Player) new JsonPlayer( data ) );
    }


    public override Task<Place> GetPlace( Coordinate coordinate )
    {
      var filepath = Path.ChangeExtension( Path.Combine( placesDirectory, coordinate.ToString() ), _extensions );

      var data = JsonDataItem.LoadData( filepath, "{ 'Building': '{EB0C8AE8-FC09-4874-9985-98C081F4D1B7}' }" );

      return Task.FromResult( (Place) new JsonPlace( data ) );
    }
  }
}