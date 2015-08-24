using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class GameDateItem
  {



    public Guid Guid { get; private set; }


    protected GameDateItem( Guid guid, JObject data )
    {
      if ( data == null )
        throw new ArgumentNullException( "data" );

      Data = (JObject) data.DeepClone();
      Guid = guid;
    }



    protected JObject Data { get; private set; }

    public override string ToString() { return Data.ToString(); }


    public override bool Equals( object obj )
    {
      var item = obj as GameDateItem;

      if ( item == null )
        return false;

      return Guid.Equals( item.Guid );
    }

    public override int GetHashCode()
    {
      return Guid.GetHashCode();
    }


    public static bool operator ==( GameDateItem a, GameDateItem b )
    {

      if ( object.ReferenceEquals( a, null ) && object.ReferenceEquals( b, null ) )
        return true;

      if ( object.ReferenceEquals( a, null ) || object.ReferenceEquals( b, null ) )
        return false;

      return a.Equals( b );
    }


    public static bool operator !=( GameDateItem a, GameDateItem b )
    {

      if ( object.ReferenceEquals( a, null ) && object.ReferenceEquals( b, null ) )
        return false;

      if ( object.ReferenceEquals( a, null ) || object.ReferenceEquals( b, null ) )
        return true;

      return a.Equals( b ) == false;
    }


    public object GetInfo()
    {
      return this;
    }



  }
}
