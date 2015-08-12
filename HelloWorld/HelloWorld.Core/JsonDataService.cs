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


      /// <summary>
      /// 保存数据对象
      /// </summary>
      public void Save()
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

      private JsonDataItem data;

      public JsonPlace( Coordinate coordinate, JsonDataItem jsonData )
      {
        _coordinate = coordinate;
        data = jsonData;
        resources = new ItemCollection( ItemListJsonConverter.FromJson( (JObject) data["Resources"] ), SaveItems );
      }

      public override BuildingDescriptor Building
      {
        get { return GameEnvironment.GetBuilding( data.GuidValue( "Building" ) ); }

        set
        {
          data["Building"] = value.Guid.ToString();
        }
      }

      private void SaveItems()
      {
        data["Resources"] = ItemListJsonConverter.ToJson( resources );
      }

      private ItemCollection resources;

      /// <summary>
      /// 资源数量
      /// </summary>
      public override ItemCollection Resources
      {
        get { return resources; }
      }



      public override Constructing Constructing
      {
        get { return null; }
        set { }
      }

      public override Producting Producting
      {
        get { return null; }
        set { }
      }



      private Coordinate _coordinate;

      public override Coordinate Coordinate { get { return _coordinate; } }

    }




    private class JsonPlayer : GamePlayer
    {
      private JsonDataItem data;

      public JsonPlayer( JsonDataItem jsonData )
      {
        data = jsonData;

        resources = new ItemCollection( ItemListJsonConverter.FromJson( (JObject) jsonData["Resources"] ), SaveItems );
      }

      private void SaveItems()
      {
        data["Resources"] = ItemListJsonConverter.ToJson( resources );
      }

      /// <summary>
      /// 昵称
      /// </summary>
      public override string Nickname
      {
        get { return data.Value<string>( "Nickname" ); }
        set { data["Nickname"] = value; }
      }

      /// <summary>
      /// 起始位置
      /// </summary>
      public override Coordinate Initiation
      {
        get { return data.CoordinateValue( "Initiation" ); }
      }

      /// <summary>
      /// 工人数量
      /// </summary>
      public override int Workers
      {
        get { return data.Value<int>( "Workers" ); }

        set { data["Workers"] = value; }
      }


      private ItemCollection resources;

      /// <summary>
      /// 资源数量
      /// </summary>
      public override ItemCollection Resources
      {
        get { return resources; }
      }


      public override Task Save()
      {
        return Task.CompletedTask;
      }
    }


    public override Task<GamePlayer> GetPlayer( Guid userId )
    {
      var filepath = Path.ChangeExtension( Path.Combine( playersDirectory, userId.ToString() ), _extensions );

      var data = JsonDataItem.LoadData( filepath, new { NickName = "Guest", Initiation = Coordinate.RandomCoordinate( 1000, 1000 ).ToString(), Resources = new ItemCollection() } );

      return Task.FromResult( (GamePlayer) new JsonPlayer( data ) );
    }


    public override Task<Place> GetPlace( Coordinate coordinate )
    {
      var filepath = Path.ChangeExtension( Path.Combine( placesDirectory, coordinate.ToString() ), _extensions );

      var data = JsonDataItem.LoadData( filepath, new { Building = Guid.Parse( "{EB0C8AE8-FC09-4874-9985-98C081F4D1B7}" ) } );

      return Task.FromResult( (Place) new JsonPlace( coordinate, data ) );
    }



  }
}