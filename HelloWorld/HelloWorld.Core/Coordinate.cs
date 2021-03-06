﻿using Newtonsoft.Json;
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
  [JsonConverter( typeof( CoordinateJsonConverter ) )]
  public sealed class Coordinate
  {


    private Coordinate() { }

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

      if ( ( y / 2 ) % 2 == 0 && x % 2 != 0 )
        throw new ArgumentException( "当坐标 y/2 是偶数时，坐标 x 必须是偶数", "x" );

      if ( ( y / 2 ) % 2 != 0 && x % 2 == 0 )
        throw new ArgumentException( "当坐标 y/2 是奇数时，坐标 x 必须是奇数", "x" );

      return new Coordinate
      {
        X = x,
        Y = y,
      };
    }


    /// <summary>
    /// 尝试创建坐标对象
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="coordinate"></param>
    /// <returns>是否成功</returns>
    public static bool TryCreate( int x, int y, out Coordinate coordinate )
    {

      coordinate = null;

      if ( y % 2 != 0 )
        return false;

      if ( ( y / 2 ) % 2 == 0 && x % 2 != 0 )
        return false;

      if ( ( y / 2 ) % 2 != 0 && x % 2 == 0 )
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
      x = random.Next( x / 2 ) * 2 + ( y / 2 ) % 2;


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



    private static readonly Dictionary<int, HashSet<Coordinate>> adjacentsCache = new Dictionary<int, HashSet<Coordinate>>()
    {
      { 1, new HashSet<Coordinate>( new[] { Coordinate.Create( -2, 0 ), Coordinate.Create( -1, 2 ), Coordinate.Create( 1, 2 ), Coordinate.Create( 2, 0 ), Coordinate.Create( 1, -2 ), Coordinate.Create( -1, -2 ) } ) }
    };

    private static object _sync = new object();


    /// <summary>
    /// 获取临近的坐标
    /// </summary>
    /// <param name="distance">距离</param>
    /// <returns></returns>
    public Coordinate[] NearlyCoordinates( int distance = 1 )
    {

      if ( distance == 0 )
        return new Coordinate[0];

      if ( distance < 0 )
        throw new ArgumentOutOfRangeException( "distance", "距离不能小于 0" );


      return GetAdjacents( distance ).Select( item => this + item ).ToArray();
    }


    private static HashSet<Coordinate> GetAdjacents( int distance )
    {

      if ( distance < 1 )
        return new HashSet<Coordinate>();

      lock ( _sync )
      {

        HashSet<Coordinate> adjacents;

        if ( adjacentsCache.TryGetValue( distance, out adjacents ) )
          return adjacents;


        var temp1 = GetAdjacents( distance - 2 );
        var temp2 = GetAdjacents( distance - 1 ).Except( temp1 );//得到外环
        adjacents = new HashSet<Coordinate>( temp2.SelectMany( item => GetAdjacents( 1 ).Select( i => i + item ) ) );
        adjacents.UnionWith( temp1 );
        adjacents.ExceptWith( new[] { Origin } );

        return adjacentsCache[distance] = adjacents;
      }

    }



    /// <summary>
    /// 将坐标转换为字符串形式
    /// </summary>
    /// <returns>坐标的字符串表达形式</returns>
    public override string ToString()
    {
      return string.Format( "({0}, {1})", X, Y );
    }


    private static Regex coordinateRegex = new Regex( @"^\s*\(\s*(?<x>[+-]?[0-9]+),\s*(?<y>[+-]?[0-9]+)\s*\)\s*$", RegexOptions.Compiled );

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



    private static readonly Coordinate _origin = Create( 0, 0 );
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
      return ( X ^ -Y );
    }



    public static bool operator ==( Coordinate a, Coordinate b )
    {

      if ( object.ReferenceEquals( a, null ) && object.ReferenceEquals( b, null ) )
        return true;

      if ( object.ReferenceEquals( a, null ) || object.ReferenceEquals( b, null ) )
        return false;

      return a.Equals( b );
    }


    public static bool operator !=( Coordinate a, Coordinate b )
    {

      if ( object.ReferenceEquals( a, null ) && object.ReferenceEquals( b, null ) )
        return false;

      if ( object.ReferenceEquals( a, null ) || object.ReferenceEquals( b, null ) )
        return true;

      return a.Equals( b ) == false;
    }



    private static readonly Coordinate nw = Create( -1, -2 );
    private static readonly Coordinate ne = Create( 1, -2 );
    private static readonly Coordinate e = Create( 2, 0 );
    private static readonly Coordinate se = Create( 1, 2 );
    private static readonly Coordinate sw = Create( -1, 2 );
    private static readonly Coordinate w = Create( -2, 0 );



    public Coordinate GetCoordinate( Direction direction )
    {
      switch ( direction )
      {
        case Direction.NorthWest:
          return this + nw;
        case Direction.NorthEast:
          return this + ne;
        case Direction.East:
          return this + e;
        case Direction.SouthEast:
          return this + se;
        case Direction.SouthWest:
          return this + sw;
        case Direction.West:
          return this + w;
        default:
          throw new InvalidOperationException();
      }
    }
  }


  public enum Direction
  {
    /// <summary>西北方向 ↖</summary>
    NorthWest,
    /// <summary>东北方向 ↗</summary>
    NorthEast,
    /// <summary>正东方向 →</summary>
    East,
    /// <summary>东南方向 ↙</summary>
    SouthEast,
    /// <summary>西南方向 ↘</summary>
    SouthWest,
    /// <summary>正西方向 ←</summary>
    West,
  }

}
