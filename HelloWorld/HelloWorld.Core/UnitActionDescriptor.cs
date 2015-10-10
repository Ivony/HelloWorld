using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  public class UnitActionDescriptor : ActionDescriptor
  {


    public UnitRestriction UnitRestriction
    {
      get; private set;
    }



    protected override void Initialize( JObject data )
    {
      base.Initialize( data );

      UnitRestriction = UnitRestriction.FromData( GameHost.GameRules, data["UnitRestriction"] );
    }


  }
}
