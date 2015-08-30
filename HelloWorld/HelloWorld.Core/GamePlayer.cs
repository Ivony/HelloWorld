using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class GamePlayer
  {


    protected GamePlayer( GameDataService service, Guid userId )
    {
      DataService = service;
      UserID = userId;
    }


    protected GameDataService DataService { get; private set; }


    /// <summary>
    /// 用户 ID
    /// </summary>
    public Guid UserID { get; private set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public abstract string Nickname { get; set; }

    /// <summary>
    /// 起始位置
    /// </summary>
    public abstract Coordinate Initiation { get; }


    /// <summary>
    /// 资源数量
    /// </summary>
    public abstract ItemCollection Resources { get; }


    /// <summary>
    /// 通知保存玩家信息修改
    /// </summary>
    /// <returns></returns>
    public abstract Task Save();


    /// <summary>
    /// 获取可以用于显示的玩家信息
    /// </summary>
    /// <returns></returns>
    public object GetInfo()
    {
      return new
      {
        Nickname,
        Resources = Resources,
      };
    }


    public override bool Equals( object obj )
    {
      var player = obj as GamePlayer;
      if ( player == null )
        return false;

      return player.UserID == UserID;
    }

    public override int GetHashCode()
    {
      return UserID.GetHashCode();
    }


    public static bool operator ==( GamePlayer player1, GamePlayer player2 )
    {

      if ( object.ReferenceEquals( player1, null ) && object.ReferenceEquals( player2, null ) )
        return true;

      if ( object.ReferenceEquals( player1, null ) || object.ReferenceEquals( player2, null ) )
        return false;

      return player1.Equals( player2 );
    }


    public static bool operator !=( GamePlayer player1, GamePlayer player2 )
    {

      if ( object.ReferenceEquals( player1, null ) && object.ReferenceEquals( player2, null ) )
        return false;

      if ( object.ReferenceEquals( player1, null ) || object.ReferenceEquals( player2, null ) )
        return true;

      return player1.Equals( player2 ) == false;
    }



  }
}
