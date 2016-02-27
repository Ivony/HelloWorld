using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public class UniqueItem : Item
  {
    public UniqueItem() : base()
    {

      identifier = Guid.NewGuid();

    }

    private Guid identifier;


    public override Guid Identifier
    {
      get { return identifier; }
    }

  }
}
