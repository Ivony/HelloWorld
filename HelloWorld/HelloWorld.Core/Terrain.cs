using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class Terrain : BaseImmovable
  {
    public new Place Place { get { return (Place) base.Place; } }

  }
}
