using System;
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


    public async Task<object> Info( string oldPassword, string newPassword )
    {
      return Host.UserService.TryResetPassword( LoginToken, oldPassword, newPassword );
    }


  }
}
