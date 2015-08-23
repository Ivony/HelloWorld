using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{



  public abstract class GameActing
  {


    protected GameActing()
    {
      Status = GameActingStatus.NotStarted;
      _sync = new object();
    }


    private object _sync = new object();

    protected object SyncRoot { get { return _sync; } }




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


          place.Acting = this;
        }

        StartOn = DateTime.UtcNow;
        Place = place;

        Status = GameActingStatus.Processing;
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

          if ( TryComplete() == false )
            return Status;

          Place.Acting = null;
        }

        return Status = GameActingStatus.Done;
      }
    }


    /// <summary>
    /// 派生类实现此方法检查活动是否已完成
    /// </summary>
    protected abstract bool TryComplete();



  }

  /// <summary>
  /// 代表正在进行的一个活动
  /// </summary>
  public class GameActing<T> : GameActing where T : GameActingDescriptor
  {

    public GameActing( T descriptor )
    {
      Descriptor = descriptor;
    }




    protected T Descriptor { get; private set; }



    protected override bool TryComplete()
    {
      return Descriptor.TryComplete( this );
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

  }
}
