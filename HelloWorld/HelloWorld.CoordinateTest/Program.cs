using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.CoordinateTest
{
  class Program
  {
    static void Main( string[] args )
    {

      for ( int i = 0; i < 1000; i++ )
      {
        Console.WriteLine( Coordinate.RandomCoordinate( 10000, 10000 ) );
      }

      Console.ReadKey();
    }
  }
}
