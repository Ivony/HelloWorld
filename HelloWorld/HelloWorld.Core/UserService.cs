using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{
  public abstract class UserService
  {

    /// <summary>
    /// 注册一个用户
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public abstract Guid Register( string email, string password );

    /// <summary>
    /// 用户登陆
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public abstract Guid? Login( string email, string password );

  }
}
