using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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



    /// <summary>
    /// 创建一个坐标对象
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
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



    /// <summary>
    /// 计算与另一个坐标的距离
    /// </summary>
    /// <param name="coordinate">要计算距离的坐标</param>
    /// <returns></returns>
    public int Distance( Coordinate coordinate )
    {
      var offsetY = Math.Abs( Y - coordinate.Y ) / 2;
      var offsetX = Math.Max( Math.Abs( X - coordinate.X ) - offsetY, 0 ) / 2;
      return offsetX + offsetY;
    }



    /// <summary>
    /// 将坐标转换为字符串形式
    /// </summary>
    /// <returns>坐标的字符串表达形式</returns>
    public override string ToString()
    {
      return string.Format( "{{{0}, {1}}}", X, Y );
    }


    private static Regex coordinateRegex = new Regex( @"^\s*\{\s*(?<x>[0-9]+),\s*(?<y>[0-9]+)\s*\}\s*$", RegexOptions.Compiled );

    /// <summary>
    /// 从字符串中解析出坐标信息
    /// </summary>
    /// <param name="expression">包含坐标信息的字符串</param>
    /// <returns>坐标信息</returns>
    public static Coordinate Parse( string expression )
    {
      if ( expression == null )
        throw new ArgumentNullException( "expression" );


      var match = coordinateRegex.Match( expression );
      if ( match.Success == false )
        throw new FormatException();

      var x = int.Parse( match.Groups["x"].Value );
      var y = int.Parse( match.Groups["y"].Value );


      return Create( x, y );
    }



    private static Coordinate _origin = Create( 0, 0 );
    /// <summary>
    /// 原点坐标
    /// </summary>
    public static Coordinate Origin { get { return _origin; } }


    /// <summary>
    /// 计算两个坐标相加后的结果
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Coordinate operator +( Coordinate a, Coordinate b )
    {
      return Create( a.X + b.X, a.Y + b.Y );
    }


    /// <summary>
    /// 计算两个坐标相减后的结果
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Coordinate operator -( Coordinate a, Coordinate b )
    {
      return Create( a.X - b.X, a.Y - b.Y );
    }


    public override bool Equals( object obj )
    {
      var coordinate = obj as Coordinate;
      if ( coordinate == null )
        return false;


      return X == coordinate.X && Y == coordinate.Y;
    }

    public override int GetHashCode()
    {
      return (X ^ -Y);
    }


  }
}
