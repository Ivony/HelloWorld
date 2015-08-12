﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;


namespace HelloWorld.WebHost
{
  [MyAuthorizeFilter]
  public class GameController : ControllerBase
  {


    [HttpGet]
    public async Task<object> Test()
    {
      return "Test OK!";
    }



    [HttpGet]
    public object Info()
    {
      return Player.GetInfo();
    }



    [HttpGet]
    public async Task<object> Info( string nickname )
    {

      Player.Nickname = nickname;
      await Player.Save();

      return Info();
    }



    [HttpGet]
    public async Task<object> Place( string coordinate )
    {
      var position = Coordinate.Parse( coordinate );
      if ( position.Distance( Coordinate.Origin ) > 1 )
        return null;


      var place = await Host.DataService.GetPlace( Player.Initiation + position );

      return place.GetInfo( Player );
    }



    public async Task<object> Info( string oldPassword, string newPassword )
    {
      return Host.UserService.TryResetPassword( LoginToken, oldPassword, newPassword );
    }


  }
}
