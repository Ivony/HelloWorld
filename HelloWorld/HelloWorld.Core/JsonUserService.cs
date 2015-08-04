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


    public JsonUserService( string rootPath )
    {
      dataRoot = rootPath;
    }



    private static readonly HashAlgorithm hash = new SHA256Managed();

    private string GetFilepath( string email )
    {
      return Path.Combine( dataRoot, Path.ChangeExtension( email, extensions ) );
    }


    public override Guid? Login( string email, string password )
    {


      if ( Path.GetInvalidFileNameChars().Any( c => email.Contains( c ) ) )
        return null;

      var data = LoadUserData( email );

      if ( data == null )
        return null;

      var encrypted = EncryptPassword( password );

      if ( data.Password != encrypted )
        return null;


      return data.UserID;
    }

    private dynamic LoadUserData( string email )
    {
      string path = GetFilepath( email );

      if ( File.Exists( path ) == false )
        return null;

      return JObject.Parse( File.ReadAllText( path ) );
    }


    private static string EncryptPassword( string password )
    {
      return Convert.ToBase64String( hash.ComputeHash( Encoding.UTF8.GetBytes( password ) ) );
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
