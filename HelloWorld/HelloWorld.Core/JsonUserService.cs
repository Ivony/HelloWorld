using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class JsonUserService : UserService
  {



    private const string extensions = ".json";
    private const string salt = "Hello";


    private readonly string userDataRoot;
    private readonly string loginDataRoot;


    public JsonUserService( string rootPath )
    {
      userDataRoot = Path.Combine( rootPath, "User" );
      loginDataRoot = Path.Combine( rootPath, "Login" );

      Directory.CreateDirectory( userDataRoot );
      Directory.CreateDirectory( loginDataRoot );
    }



    private static readonly HashAlgorithm hash = new SHA256Managed();

    private string GetFilepath( string email )
    {
      return Path.Combine( userDataRoot, Path.ChangeExtension( email, extensions ) );
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
      return Convert.ToBase64String( hash.ComputeHash( Encoding.UTF8.GetBytes( password + salt ) ) );
    }



    private string CreateLoginToken( Guid userID )
    {

      foreach ( var f in Directory.EnumerateFiles( loginDataRoot ).Where( file => File.GetCreationTimeUtc( file ) < DateTime.UtcNow.AddDays( -1 ) ) )
      {
        try
        {
          File.Delete( f );
        }
        catch { }
      }

      var token = Path.GetRandomFileName();
      File.WriteAllText( Path.Combine( loginDataRoot, token ), userID.ToString() );
      return token;
    }



    public override Guid? GetUserID( string loginToken )
    {
      if ( string.IsNullOrEmpty( loginToken ) )
        return null;

      var path = Path.Combine( loginDataRoot, loginToken );
      if ( File.Exists( path ) == false )
        return null;

      Guid userId;
      if ( Guid.TryParse( File.ReadAllText( path ), out userId ) == false )
        return null;

      return userId;
    }




    public override bool TryLogin( string email, string password, out string loginToken )
    {

      loginToken = null;

      if ( Path.GetInvalidFileNameChars().Any( c => email.Contains( c ) ) )
        return false;

      var data = LoadUserData( email );

      if ( data == null )
        return false;

      var encrypted = EncryptPassword( password );

      if ( data.Password != encrypted )
        return false;


      loginToken = CreateLoginToken( (Guid) data.UserID );
      return true;
    }



    /// <summary>
    /// 尝试注册一个用户
    /// </summary>
    /// <param name="email">邮箱</param>
    /// <param name="password">密码</param>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    public override bool TryRegister( string email, string password, out string loginToken )
    {
      loginToken = null;

      var path = GetFilepath( email );

      if ( File.Exists( path ) )
        return false;


      var encrypted = EncryptPassword( password );

      var data = (dynamic) new JObject();

      data.Email = email;
      data.Password = encrypted;
      data.UserID = Guid.NewGuid();


      File.WriteAllText( path, ((JObject) data).ToString() );
      loginToken = CreateLoginToken( (Guid) data.UserID );
      return true;
    }
  }
}
