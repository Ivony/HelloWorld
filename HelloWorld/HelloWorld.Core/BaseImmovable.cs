using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class BaseImmovable : Immovable
  {
    internal override void InitializeData( PlaceBase place, JsonDataObject data )
    {
      if ( place is Place )
        base.InitializeData( place, data );

      else
        throw new InvalidOperationException();
    }

    public new Place Place { get { return (Place) base.Place; } }

  }
}
