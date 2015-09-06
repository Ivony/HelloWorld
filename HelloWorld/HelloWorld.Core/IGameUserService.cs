using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
  public interface IGameUserService
  {

    /// <summary>
    /// 注册一个用户
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    bool TryRegister( string email, string password, out string loginToken );

    /// <summary>
    /// 用户登陆
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    bool TryLogin( string email, string password, out string loginToken );


    /// <summary>
    /// 通过登录凭证获取用户ID
    /// </summary>
    /// <param name="loginToken"></param>
    Guid? GetUserID( string loginToken );

    /// <summary>
    /// 尝试修改密码
    /// </summary>
    /// <param name="loginToken">登陆凭据</param>
    /// <param name="oldPassword">旧密码</param>
    /// <param name="newPassword">新密码</param>
    /// <returns></returns>
    bool TryResetPassword( string loginToken, string oldPassword, string newPassword );
  }
}
