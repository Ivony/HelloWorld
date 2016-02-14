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
      Directory.CreateDirectory( unitsDirectory = Path.Combine( DataRoot, "Units" ) );
      Directory.CreateDirectory( placesDirectory = Path.Combine( DataRoot, "Places" ) );
      Directory.CreateDirectory( playersDirectory = Path.Combine( DataRoot, "Players" ) );
    }



    private readonly string placesDirectory;
    private readonly string playersDirectory;
    private readonly string unitsDirectory;


    private const string _extensions = ".json";



    /// <summary>
    /// 数据文件根路径
    /// </summary>
    public string DataRoot
    {
      get;
      private set;
    }



    private INameService _nameService = new DefaultNameService();

    /// <summary>
    /// 获取名称服务
    /// </summary>
    public INameService NameService
    {
      get { return _nameService; }
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
    private Dictionary<Coordinate, Place> places = new Dictionary<Coordinate, Place>();


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

        Place place;
        if ( places.TryGetValue( coordinate, out place ) )
          return place;


        var filepath = Path.ChangeExtension( Path.Combine( placesDirectory, coordinate.ToString() ), _extensions );
        var data = JsonDataItem.LoadData( filepath, new { CheckPoint = DateTime.UtcNow } );

        place = GameHost.GameRules.CreatePlace( coordinate );
        place.InitializeData( this, data );

        return place;
      }
    }

    /// <summary>
    /// 获取指定地块所有单位
    /// </summary>
    /// <param name="coordinate">地块坐标</param>
    /// <returns></returns>
    public Unit[] GetUnits( Coordinate coordinate )
    {
      lock ( _unitSync )
      {

        HashSet<Unit> units;

        if ( _placeUnits.TryGetValue( coordinate, out units ) )
        {
          var invalids = units.Where( item => item.Coordinate != coordinate ).ToArray();//检查单位是否还在这个坐标。
          foreach ( var item in invalids )
            units.Remove( item );

          return units.ToArray();
        }
        else
          return new Unit[0];

      }
    }



    /// <summary>
    /// 获取指定玩家所有单位
    /// </summary>
    /// <param name="player">玩家对象</param>
    /// <returns></returns>
    public Unit[] GetUnits( GamePlayer player )
    {
      lock ( _unitSync )
      {

        HashSet<Unit> units;

        if ( _playerUnits.TryGetValue( player.Guid, out units ) )
        {
          var invalids = units.Where( item => item.Owner != player.Guid ).ToArray();//检查单位是否还属于这个玩家。
          foreach ( var item in invalids )
            units.Remove( item );

          return units.ToArray();
        }
        else
          return new Unit[0];

      }
    }


    /// <summary>
    /// 保存游戏数据对象
    /// </summary>
    /// <param name="dataItem"></param>
    public void Save( GameDataItem dataItem )
    {
      var unit = dataItem as Unit;

      if ( unit != null )
      {
        Save( unit );
        return;
      }


      var place = dataItem as Place;

      if ( place != null )
      {
        Save( place );
      }
    }



    /// <summary>
    /// 保存游戏地块对象
    /// </summary>
    /// <param name="place"></param>
    private void Save( Place place )
    {
      var filepath = Path.Combine( placesDirectory, place.Coordinate + ".json" );
      File.WriteAllText( filepath, place.SaveAsJson() );
    }


    /// <summary>
    /// 保存游戏单位对象
    /// </summary>
    /// <param name="unit"></param>
    private void Save( Unit unit )
    {
      RefreshUnitCahce( unit );
      var filepath = Path.Combine( unitsDirectory, unit.Guid + ".json" );
      File.WriteAllText( filepath, unit.SaveAsJson() );
    }



    private object _unitSync = new object();


    private Dictionary<Guid, HashSet<Unit>> _playerUnits = new Dictionary<Guid, HashSet<Unit>>();

    private Dictionary<Coordinate, HashSet<Unit>> _placeUnits = new Dictionary<Coordinate, HashSet<Unit>>();






    /// <summary>
    /// 初始化游戏数据服务
    /// </summary>
    void IGameDataService.Initialize()
    {
      InitializeUnits();
    }


    private void InitializeUnits()
    {
      foreach ( var file in Directory.EnumerateFiles( Path.Combine( DataRoot, "units" ) ) )
      {
        var data = JObject.Parse( File.ReadAllText( file ) );

        var unit = GameHost.GameRules.CreateInstance<Unit>( (string) data["Type"] );
        unit.InitializeData( this, data );

        RefreshUnitCahce( unit );
      }
    }

    private void RefreshUnitCahce( Unit unit )
    {

      lock ( _unitSync )
      {

        HashSet<Unit> set;

        if ( _playerUnits.TryGetValue( unit.Owner, out set ) == false )
          _playerUnits[unit.Owner] = set = new HashSet<Unit>();

        set.Add( unit );



        if ( _placeUnits.TryGetValue( unit.Coordinate, out set ) == false )
          _placeUnits[unit.Coordinate] = set = new HashSet<Unit>();

        set.Add( unit );
      }
    }
  }
}
