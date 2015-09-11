using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class TextFileMessageService : IGameMessageService
  {

    public TextFileMessageService( string path )
    {

      RootPath = Path.Combine( path, "Messages" );
      Directory.CreateDirectory( RootPath );

    }
    public string RootPath { get; private set; }


    public void AddMessage( Guid playerId, GameMessageEntry message )
    {

      var filepath = Path.Combine( RootPath, playerId + ".message" );
      File.AppendAllLines( filepath, new[] { GetText( message ) } );


    }

    private string GetText( GameMessageEntry message )
    {
      return string.Format( "{0:O} {1}", message.NotifyTime, message.Content );
    }

    public void AddAnnouncement( GameMessageEntry message )
    {
      var filepath = Path.Combine( RootPath, "annoucement.message" );
      File.AppendAllLines( filepath, new[] { GetText( message ) } );
    }

    public IEnumerable<GameMessageEntry> GetMessages( Guid playerId, DateTime startTime )
    {


      var messages = LoadMessages( Path.Combine( RootPath, playerId + ".message" ) );
      messages = messages.Concat( LoadMessages( Path.Combine( RootPath, "annoucement.message" ) ) ).OrderBy( item => item.NotifyTime ).ToArray();


      return messages.OrderBy( item => item.NotifyTime ).SkipWhile( item => item.NotifyTime < startTime );
    }

    private GameMessageEntry[] LoadMessages( string filepath )
    {

      return ReadAllLines( filepath ).Select( text => FromText( text ) ).ToArray();


    }



    private GameMessageEntry FromText( string text )
    {

      var dateString = text.Remove( 29 );
      var content = text.Substring( 29 );

      var date = DateTime.ParseExact( dateString, "O", null );

      return new GameMessageEntry( date, content );
    }




    private string[] ReadAllLines( string filepath )
    {

      using ( var stream = File.Open( filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite ) )
      {

        var reader = new StreamReader( stream );

        var list = new Queue<string>();

        while ( true )
        {
          var text = reader.ReadLine();
          if ( text == null )
            break;

          list.Enqueue( text );
        }


        if ( list.Count < 150 )
          return list.ToArray();


        while ( list.Count > 100 )
        {
          list.Dequeue();
        }

        var result = list.ToArray();



        stream.SetLength( 0 );//清空文件重新写入

        var writer = new StreamWriter( stream );

        foreach ( var text in result )
          writer.WriteLine( text );


        return result;

      }
    }
  }
}
