using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class GameItemData
  {



    public Guid Guid { get; private set; }


    protected GameItemData( Guid guid, JObject data )
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
      var item = obj as GameItemData;

      if ( item == null )
        return false;

      return Guid.Equals( item.Guid );
    }

    public override int GetHashCode()
    {
      return Guid.GetHashCode();
    }
  }
}
