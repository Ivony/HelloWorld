﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  internal interface IDataHost
  {



    Guid Token { get; }

    void Save( JToken data );

  }
}
