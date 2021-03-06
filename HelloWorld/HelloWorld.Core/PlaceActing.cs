﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{



  public sealed class PlaceActing
  {


    private object _sync = new object();


    /// <summary>
    /// 用于同步的对象
    /// </summary>
    public object SyncRoot { get { return _sync; } }




    /// <summary>
    /// 活动的描述
    /// </summary>
    public ActionDescriptorBase ActionDescriptor { get; private set; }


    /// <summary>
    /// 在指定地块开始这个活动
    /// </summary>
    /// <param name="place">要开始活动的地块</param>
    internal static PlaceActing StartAt( GamePlayer player, Place place, ActionDescriptorBase action )
    {

      if ( place == null )
        throw new ArgumentNullException( "place" );


      var acting = new PlaceActing
      {
        StartOn = DateTime.UtcNow,
        ActionDescriptor = action,
        PlayerID = player.Guid,
        Place = place,
      };


      place.SetActing( acting );
      return acting;
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
    /// 发起活动的玩家ID
    /// </summary>
    public Guid? PlayerID { get; private set; }


    /// <summary>
    /// 获取发起活动的玩家对象
    /// </summary>
    /// <returns></returns>
    public GamePlayer GetPlayer()
    {
      if ( PlayerID == null )
        return null;

      return GameHost.DataService.GetPlayer( PlayerID.Value );
    }

    /// <summary>
    /// 检查活动状态
    /// </summary>
    public void Check( DateTime now )
    {
      lock ( SyncRoot )
      {
        if ( Place == null )
          throw new InvalidOperationException();


        if ( this.Equals( Place.Acting ) == false )
          throw new InvalidOperationException();

        if ( ActionDescriptor.TryComplete( this, now ) )
        {
          Place.SetActing( null );
        }
      }
    }



    public JObject ToJson()
    {

      return JObject.FromObject( new
      {
        StartOn,
        PlayerID,
        ActionDescriptor = ActionDescriptor.Guid,
      } );
    }




    private PlaceActing() { }


    /// <summary>
    /// 从 JSON 数据中读取
    /// </summary>
    /// <param name="place">活动所处地块</param>
    /// <param name="data">活动数据</param>
    /// <returns>活动对象</returns>
    public static PlaceActing FromData( Place place, JObject data )
    {

      if ( place == null )
        throw new ArgumentNullException( "place" );

      if ( data == null )
        return null;

      var startOn = data.Value<DateTime>( "StartOn" );
      var playerId = data.GuidValue( "PlayerID" );
      var action = GameHost.GameRules.GetDataItem<ActionDescriptorBase>( data.GuidValue( "ActionDescriptor" ) );

      if ( action == null )
        return null;



      return new PlaceActing
      {
        PlayerID = playerId,
        Place = place,
        StartOn = startOn,
        ActionDescriptor = action,
      };
    }



    public override bool Equals( object obj )
    {
      var acting = obj as PlaceActing;
      if ( acting == null )
        return false;



      if ( acting.Place != this.Place )
        return false;

      if ( acting.ActionDescriptor != this.ActionDescriptor )
        throw new InvalidOperationException();

      return true;
    }


    public override int GetHashCode()
    {
      return Place.GetHashCode() ^ ActionDescriptor.GetHashCode();
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
      } );

      var descriptor = ActionDescriptor as ActionDescriptor;
      if ( descriptor != null )
        data["Remaining"] = ( StartOn + descriptor.Requirement.Time ) - DateTime.UtcNow;

      return data;
    }


  }
}
