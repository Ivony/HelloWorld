using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Core
{
  public class JsonUserService : UserService
  {



    private readonly string dataRoot;
    private const string extensions = ".json";
    private const string salt = "Hello";


    public JsonUserService( string rootPath )
    {
      dataRoot = rootPath;
    }



    private static readonly HashAlgorithm hash = new SHA256Managed();

    private string GetFilepath( string email )
    {
      return Path.Combine( dataRoot, Path.ChangeExtension( email, extensions ) );
    }


    public override bool TryLogin( string email, string password, out Guid userId )
    {

      userId = Guid.Empty;

      if ( Path.GetInvalidFileNameChars().Any( c => email.Contains( c ) ) )
        return false;

      var data = LoadUserData( email );

      if ( data == null )
        return false;

      var encrypted = EncryptPassword( password );

      if ( data.Password != encrypted )
        return false;


      userId = data.UserID;
      return true;
    }

    private dynamic LoadUserData( string email )
    {
      string path = GetFilepath( email );

      if ( File.Exists( path ) == false )
        return false;

      return JObject.Parse( File.ReadAllText( path ) );
    }


    private static string EncryptPassword( string password )
    {
      return Convert.ToBase64String( hash.ComputeHash( Encoding.UTF8.GetBytes( password + salt ) ) );
    }



    /// <summary>
    /// 尝试注册一个用户
    /// </summary>
    /// <param name="email">邮箱</param>
    /// <param name="password">密码</param>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    public override bool TryRegister( string email, string password, out Guid userId )
    {

      userId = Guid.Empty;

      var path = GetFilepath( email );

      if ( File.Exists( path ) )
        return false;


      var encrypted = EncryptPassword( password );

      var data = (dynamic) new JObject();

      data.Email = email;
      data.Password = encrypted;
      data.UserID = userId = Guid.NewGuid();


      File.WriteAllText( path, ((JObject) data).ToString() );
      return true;
    }
  }
}
