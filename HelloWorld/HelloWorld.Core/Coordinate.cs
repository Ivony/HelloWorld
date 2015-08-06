using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{


  /// <summary>
  /// 坐标
  /// </summary>
  public sealed class Coordinate
  {
    public int X { get; private set; }

    public int Y { get; private set; }



    public static Coordinate Create( int x, int y )
    {

      if ( y % 2 != 0 )
        throw new ArgumentException( "坐标 y 必须是偶数", "y" );

      if ( (y / 2) % 2 == 0 && x % 2 != 0 )
        throw new ArgumentException( "当坐标 y/2 是偶数时，坐标 x 必须是偶数", "x" );

      if ( (y / 2) % 2 != 0 && x % 2 == 0 )
        throw new ArgumentException( "当坐标 y/2 是奇数时，坐标 x 必须是奇数", "x" );

      return new Coordinate
      {
        X = x,
        Y = y,
      };
    }


    public static bool TryCreate( int x, int y, out Coordinate coordinate )
    {

      coordinate = null;

      if ( y % 2 != 0 )
        return false;

      if ( (y / 2) % 2 == 0 && x % 2 != 0 )
        return false;

      if ( (y / 2) % 2 != 0 && x % 2 == 0 )
        return false;

      coordinate = new Coordinate
      {
        X = x,
        Y = y,
      };

      return true;
    }


    private static Random random = new Random( DateTime.Now.Millisecond );



    /// <summary>
    /// 取一个不超出指定范围的随机坐标
    /// </summary>
    /// <param name="x">x取值范围</param>
    /// <param name="y">y取值范围</param>
    /// <returns></returns>
    public static Coordinate RandomCoordinate( int x, int y )
    {

      y = random.Next( y / 2 ) * 2;
      x = random.Next( x / 2 ) * 2 + (y / 2) % 2;


      return Create( x, y );
    }


    public override string ToString()
    {
      return string.Format( "{0}, {1}", X, Y );
    }


  }
}
