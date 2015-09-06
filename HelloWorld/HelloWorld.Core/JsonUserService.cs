using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class JsonUserService : IGameUserService
  {
    private const string salt = "Hello";


    private readonly string userDataRoot;
    private readonly string loginDataRoot;


    private static readonly Regex emailRegex = new Regex( @"^[0-9a-zA-Z-\.]+@([0-9a-zA-Z-]+\.)+[0-9a-zA-Z-]+$", RegexOptions.Compiled );

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
      return Path.Combine( userDataRoot, email + ".json" );
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



    private string CreateLoginToken( Guid userID, string email )
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

      var data = JObject.FromObject( new { UserID = userID, Email = email } );
      File.WriteAllText( Path.Combine( loginDataRoot, token ), data.ToString() );
      return token;
    }



    public override Guid? GetUserID( string loginToken )
    {

      var data = LoadLoginData( loginToken );
      if ( data == null )
        return null;

      return data.GuidValue( "UserID" );
    }


    private JObject LoadLoginData( string loginToken )
    {
      if ( string.IsNullOrEmpty( loginToken ) )
        return null;

      var path = Path.Combine( loginDataRoot, loginToken );
      if ( File.Exists( path ) == false )
        return null;


      return JObject.Parse( File.ReadAllText( path ) );

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


      loginToken = CreateLoginToken( (Guid) data.UserID, email );
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

      if ( emailRegex.IsMatch( email ) == false )
        throw new FormatException( "invalid email address." );


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
      loginToken = CreateLoginToken( (Guid) data.UserID, email );
      return true;
    }


    /// <summary>
    /// 尝试重设用户密码
    /// </summary>
    /// <param name="loginToken"></param>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    public override bool TryResetPassword( string loginToken, string oldPassword, string newPassword )
    {

      var loginData = LoadLoginData( loginToken );
      if ( loginData == null )
        return false;


      var path = GetFilepath( loginData.Value<string>( "Email" ) );

      if ( File.Exists( path ) == false )
        return false;

      var userData = JObject.Parse( File.ReadAllText( path ) );
      if ( EncryptPassword( oldPassword ) != userData.Value<string>( "Password" ) )
        return false;

      userData["Password"] = EncryptPassword( newPassword );
      File.WriteAllText( path, userData.ToString() );
      return true;
    }
  }
}
