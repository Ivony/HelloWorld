using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{



  public class GameActing
  {


    public GameActing( GameActingDescriptor descriptor )
    {
      ActingDescriptor = descriptor;
      Status = GameActingStatus.NotStarted;
      _sync = new object();
    }


    private object _sync = new object();

    protected object SyncRoot { get { return _sync; } }


    /// <summary>
    /// 活动的描述
    /// </summary>
    public GameActingDescriptor ActingDescriptor { get; private set; }



    /// <summary>
    /// 在指定地块开始这个活动
    /// </summary>
    /// <param name="place">要开始活动的地块</param>
    internal void StartAt( Place place )
    {

      lock ( SyncRoot )
      {

        if ( place == null )
          throw new ArgumentNullException( "place" );

        if ( Status != GameActingStatus.NotStarted )
          throw new InvalidOperationException();


        lock ( place.SyncRoot )
        {

          if ( place.Owner == null )
            throw new ArgumentException( "不能在无主土地上开始", "place" );

          if ( place.Acting != null )
            throw new ArgumentException( "土地上已经存在一个正在进行的任务", "place" );



          StartOn = DateTime.UtcNow;
          Place = place;
          Status = GameActingStatus.Processing;


          place.Acting = this;
        }

      }
    }




    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartOn { get; private set; }

    /// <summary>
    /// 所在位置
    /// </summary>
    public Place Place { get; private set; }

    /// <summary>
    /// 活动状态
    /// </summary>
    public GameActingStatus Status { get; protected set; }



    /// <summary>
    /// 检查活动状态
    /// </summary>
    public GameActingStatus Check()
    {
      lock ( SyncRoot )
      {
        if ( Status == GameActingStatus.NotStarted )
          return Status;


        lock ( Place )
        {
          if ( Place.Acting != this )
            throw new InvalidOperationException();

          if ( ActingDescriptor.TryComplete( this ) == false )
            return Status;

          Place.Acting = null;
        }

        return Status = GameActingStatus.Done;
      }
    }



    public JObject ToJson()
    {

      return JObject.FromObject( new
      {
        StartOn,
        Place = Place == null ? null : Place.Coordinate,
        ActingDescriptor = ActingDescriptor.Guid,
        Status = Status.ToString(),
      } );
    }




    private GameActing() { }


    /// <summary>
    /// 从 JSON 数据中读取
    /// </summary>
    /// <param name="jObject"></param>
    /// <returns></returns>
    public static GameActing FromData( GameDataService dataService, JObject data )
    {

      if ( data == null )
        return null;


      Place place = null;
      if ( data["Place"] != null && data["Place"].Type != JTokenType.Null )
        place = dataService.GetPlace( data.CoordinateValue( "Place" ) );


      var startOn = data.Value<DateTime>( "StartOn" );
      var acting = GameEnvironment.GetDataItem<GameActingDescriptor>( data.GuidValue( "ActingDescriptor" ) );
      var status = GameActingStatus.GetStatus( data.Value<string>( "Status" ) );


      return new GameActing
      {
        StartOn = startOn,
        Place = place,
        ActingDescriptor = acting,
        Status = status,
      };


      throw new NotImplementedException();
    }




    public object GetInfo()
    {
      return new
      {
        ActingDescriptor = ActingDescriptor.GetInfo(),
        StartOn,
        Status = Status.ToString(),
      };
    }

  }





  /// <summary>
  /// 定义游戏活动状态
  /// </summary>
  public sealed class GameActingStatus
  {


    /// <summary>
    /// 尚未开始
    /// </summary>
    public static readonly GameActingStatus NotStarted = new GameActingStatus( "NotStarted" );

    /// <summary>
    /// 正在进行
    /// </summary>
    public static readonly GameActingStatus Processing = new GameActingStatus( "Processing" );

    /// <summary>
    /// 已经完成
    /// </summary>
    public static readonly GameActingStatus Done = new GameActingStatus( "Done" );


    private string status;

    private GameActingStatus( string status )
    {
      this.status = status;
    }


    public override bool Equals( object obj )
    {
      var a = obj as GameActingStatus;
      if ( a == null )
        return false;


      return a.status == status;
    }


    public override int GetHashCode()
    {
      return status.GetHashCode();
    }

    public override string ToString()
    {
      return status;
    }


    public static GameActingStatus GetStatus( string str )
    {
      if ( str == null )
        throw new ArgumentNullException( "str" );

      else if ( str.Equals( NotStarted.status, StringComparison.OrdinalIgnoreCase ) )
        return NotStarted;

      else if ( str.Equals( Processing.status, StringComparison.OrdinalIgnoreCase ) )
        return Processing;

      else if ( str.Equals( Done.status, StringComparison.OrdinalIgnoreCase ) )
        return Done;

      else
        throw new InvalidOperationException();

    }
  }
}
