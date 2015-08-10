using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloWorld.Test
{
  [TestClass]
  public class CoordinateTest
  {
    [TestMethod]
    public void RandomTest()
    {
      for ( int i = 0; i < 10000; i++ )
      {
        Console.WriteLine( Coordinate.RandomCoordinate( 10000, 10000 ) );
      }
    }


    [TestMethod]
    public void DistanceTest()
    {

      var a = Coordinate.Create( 1, 6 );

      Assert.AreEqual( a.Distance( Coordinate.Create( 0, 0 ) ), 3 );
      Assert.AreEqual( a.Distance( Coordinate.Create( 5, 2 ) ), 3 );
      Assert.AreEqual( a.Distance( Coordinate.Create( 7, 2 ) ), 4 );
      Assert.AreEqual( a.Distance( Coordinate.Create( 4, 0 ) ), 3 );
      Assert.AreEqual( a.Distance( Coordinate.Create( 2, 0 ) ), 3 );


    }

  }
}
