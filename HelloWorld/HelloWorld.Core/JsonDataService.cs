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
  public class JsonDataService : IGameDataService
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



    /// <summary>
    /// 数据文件根路径
    /// </summary>
    public string DataRoot
    {
      get;
      private set;
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
        File.WriteAllText( _filepath, ( (JObject) this ).ToString( Formatting.None ) );
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


      public JsonPlace( JsonDataService service, Coordinate coordinate, JsonDataItem jsonData )
        : base( service, coordinate )
      {
        data = jsonData;
        resources = new ItemCollection( ItemListJsonConverter.FromJson( (JObject) data["Resources"] ), collection => SaveResources() );
      }


      /// <summary>
      /// 地块上的建筑/地形
      /// </summary>
      public override BuildingDescriptor Building
      {
        get { return GameHost.GameRules.GetDataItem<BuildingDescriptor>( data.GuidValue( "Building" ) ); }
        set { data["Building"] = value.Guid.ToString( "D" ); }
      }


      /// <summary>
      /// 地块的所有者
      /// </summary>
      public override GamePlayer Owner
      {
        get
        {
          var owner = data.GuidValue( "Owner" );
          if ( owner == null )
            return null;

          else
            return DataService.GetPlayer( owner.Value );
        }
        set
        {
          if ( value == null )
            data["Owner"] = null;
          else
            data["Owner"] = value.UserID;
        }
      }




      public override PlaceActing Acting
      {
        get
        {
          if ( data["Acting"] == null || data["Acting"].Type == JTokenType.Null )
            return null;
          else
            return PlaceActing.FromData( this, (JObject) data["Acting"] );
        }
        set
        {
          if ( value == null )
            data["Acting"] = null;
          else
            data["Acting"] = value.ToJson();
        }
      }

      public override Unit Unit
      {
        get
        {
          if ( data["Unit"] == null || data["Unit"].Type == JTokenType.Null )
            return null;
          else
            return Unit.FromData( this, (JObject) data["Unit"] );
        }
        set
        {
          if ( value == null )
            data["Unit"] = null;
          else
            data["Unit"] = value.ToJson();
        }
      }





      public override DateTime CheckPoint
      {
        get { return data.Value<DateTime>( "CheckPoint" ); }
        set { data["CheckPoint"] = value; }
      }




      /// <summary>
      /// 保存 Resources 属性
      /// </summary>
      private void SaveResources()
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

    }




    private class JsonPlayer : GamePlayer
    {

      private JsonDataItem data;

      public JsonPlayer( JsonDataService service, Guid userId, JsonDataItem jsonData )
        : base( service, userId )
      {
        data = jsonData;
        resources = new ItemCollection( ItemListJsonConverter.FromJson( (JObject) jsonData["Resources"] ), collection => SaveItems() );
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


      internal void Init()
      {
        if ( data["Init"] != null )
        {
          GameHost.GameRules.InitializePlayer( this );
          data.Remove( "Init" );
          data.Save();
        }
      }
    }



    private object _sync = new object();
    private Dictionary<Guid, JsonPlayer> players = new Dictionary<Guid, JsonPlayer>();
    private Dictionary<Coordinate, JsonPlace> places = new Dictionary<Coordinate, JsonPlace>();


    /// <summary>
    /// 获取一个玩家对象
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    public GamePlayer GetPlayer( Guid userId )
    {
      lock ( _sync )
      {

        JsonPlayer player;
        if ( players.TryGetValue( userId, out player ) )
          return player;


        var filepath = Path.ChangeExtension( Path.Combine( playersDirectory, userId.ToString( "D" ) ), _extensions );

        var data = JsonDataItem.LoadData( filepath, new { Nickname = "Guest", Initiation = GameHost.GameRules.GetInitiation(), Init = true, Resources = new ItemCollection() } );


        player = new JsonPlayer( this, userId, data );
        player.Init();


        return players[userId] = player;


      }
    }


    /// <summary>
    /// 获取一个地块对象
    /// </summary>
    /// <param name="coordinate">地块坐标</param>
    /// <returns></returns>
    public Place GetPlace( Coordinate coordinate )
    {
      lock ( _sync )
      {

        JsonPlace place;
        if ( places.TryGetValue( coordinate, out place ) )
          return place;


        var filepath = Path.ChangeExtension( Path.Combine( placesDirectory, coordinate.ToString() ), _extensions );

        var data = JsonDataItem.LoadData( filepath, new { Building = GameHost.GameRules.InitiationBuilding.Guid, CheckPoint = DateTime.UtcNow } );

        return places[coordinate] = new JsonPlace( this, coordinate, data );
      }
    }



  }
}