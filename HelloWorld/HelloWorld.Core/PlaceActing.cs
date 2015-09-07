using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{



  public class PlaceActing
  {


    public PlaceActing( ActionDescriptorBase descriptor, Action<PlaceActing> changeHandler = null )
    {

      if ( descriptor == null )
        throw new ArgumentNullException( "descriptor" );

      ActionDescriptor = descriptor;
      Status = GameActingStatus.NotStarted;
      ChangeHandler = changeHandler;
      _sync = new object();
    }


    private object _sync = new object();

    protected object SyncRoot { get { return _sync; } }


    /// <summary>
    /// 当发生修改时需要调用的方法
    /// </summary>
    protected Action<PlaceActing> ChangeHandler { get; private set; }



    /// <summary>
    /// 活动的描述
    /// </summary>
    public ActionDescriptorBase ActionDescriptor { get; private set; }



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



        StartOn = DateTime.UtcNow;
        Place = place;
        Status = GameActingStatus.Processing;


        place.Acting = this;
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
        if ( Status == GameActingStatus.NotStarted || Status == GameActingStatus.Done )
          return Status;


        if ( this.Equals( Place.Acting ) == false )
          throw new InvalidOperationException();

        if ( ActionDescriptor.TryComplete( this ) == false )
          return Status;

        Place.Acting = null;


        Status = GameActingStatus.Done;
        OnChanged();

        return Status;
      }
    }



    protected void OnChanged()
    {
      if ( ChangeHandler != null )
        ChangeHandler( this );
    }




    public JObject ToJson()
    {

      return JObject.FromObject( new
      {
        StartOn,
        Place = Place == null ? null : Place.Coordinate,
        ActionDescriptor = ActionDescriptor.Guid,
        Status = Status.ToString(),
      } );
    }




    private PlaceActing() { }


    /// <summary>
    /// 从 JSON 数据中读取
    /// </summary>
    /// <param name="jObject"></param>
    /// <returns></returns>
    public static PlaceActing FromData( GameDataService dataService, JObject data )
    {

      if ( data == null )
        return null;


      Place place = null;
      if ( data["Place"] != null && data["Place"].Type != JTokenType.Null )
        place = dataService.GetPlace( data.CoordinateValue( "Place" ) );


      var startOn = data.Value<DateTime>( "StartOn" );
      var action = GameHost.GameRules.GetDataItem<ActionDescriptorBase>( data.GuidValue( "ActionDescriptor" ) );
      var status = GameActingStatus.GetStatus( data.Value<string>( "Status" ) );

      if ( action == null )
        return null;



      return new PlaceActing
      {
        StartOn = startOn,
        Place = place,
        ActionDescriptor = action,
        Status = status,
      };
    }



    public override bool Equals( object obj )
    {
      var acting = obj as PlaceActing;
      if ( acting == null )
        return false;


      if ( acting.Status != this.Status )
        return false;


      if ( Status == GameActingStatus.Processing )
      {

        if ( acting.Place != this.Place )
          return false;

        if ( acting.ActionDescriptor != this.ActionDescriptor )
          throw new InvalidOperationException();

        return true;
      }

      return acting.ActionDescriptor == this.ActionDescriptor;
    }


    public override int GetHashCode()
    {
      return Status.GetHashCode() ^ ActionDescriptor.GetHashCode() ^ Place.GetHashCode();
    }




    /// <summary>
    /// 获取可以展示给玩家的信息
    /// </summary>
    /// <returns>可以展示给玩家的信息</returns>
    public object GetInfo()
    {
      var data = JObject.FromObject( new
      {
        ActionDescriptor = ActionDescriptor.GetInfo(),
        StartOn,
        Status = Status.ToString(),
      } );

      var descriptor = ActionDescriptor as ActionDescriptor;
      if ( descriptor != null )
        data["Remaining"] = ( StartOn + descriptor.Requirment.Time ) - DateTime.UtcNow;

      return data;
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
