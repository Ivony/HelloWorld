﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public static class GameExtensions
  {

    /// <summary>
    /// 协助解析类型的帮助方法
    /// </summary>
    /// <param name="rules">游戏规则集</param>
    /// <param name="typeName"></param>
    /// <param name="restrict"></param>
    /// <param name="defaultType"></param>
    /// <returns></returns>
    public static Type GetType( this GameRulesBase rules, string typeName, Type restrict, Type defaultType = null )
    {

      if ( restrict == null )
        throw new ArgumentNullException( "restrict" );


      var type = defaultType ?? restrict;


      if ( string.IsNullOrWhiteSpace( typeName ) == false )
        type = rules.GetType( typeName );


      if ( type == null )
        throw new Exception( string.Format( "type {0} is not found.", typeName ) );


      if ( restrict != null && restrict.IsAssignableFrom( type ) == false )
        throw new Exception( string.Format( "type {0} is not unit instance type, data load failed.", typeName ) );

      return type;
    }



    /// <summary>
    /// 协助创建指定类型实例的方法
    /// </summary>
    /// <typeparam name="T">实例类型</typeparam>
    /// <param name="rules">游戏规则集</param>
    /// <param name="typeName">类型名称</param>
    /// <returns></returns>
    public static T CreateInstance<T>( this GameRulesBase rules, string typeName )
    {
      var type = rules.GetType( typeName );
      return (T) Activator.CreateInstance( type );
    }



    /// <summary>
    /// 将绝对坐标转换为相对坐标
    /// </summary>
    /// <param name="coordinate">要转换的绝对坐标</param>
    /// <returns></returns>
    public static Coordinate ToRelative( this Coordinate coordinate, GamePlayer player )
    {
      return coordinate - player.Initiation;
    }


    /// <summary>
    /// 将相对坐标转换为绝对坐标
    /// </summary>
    /// <param name="coordinate">要转换的相对坐标</param>
    public static Coordinate ToAbsolute( this Coordinate coordinate, GamePlayer player )
    {
      return coordinate + player.Initiation;
    }


  }
}
