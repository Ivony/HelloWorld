using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class UserService
  {

    /// <summary>
    /// 注册一个用户
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public abstract bool TryRegister( string email, string password, out string loginToken );

    /// <summary>
    /// 用户登陆
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public abstract bool TryLogin( string email, string password, out string loginToken );


    /// <summary>
    /// 通过登录凭证获取用户ID
    /// </summary>
    /// <param name="loginToken"></param>
    public abstract Guid? GetUserID( string loginToken );

  }
}
