using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{


  /// <summary>
  /// 游戏全局规则和对象管理器实现基类
  /// </summary>
  public abstract class GameRulesBase
  {


    /// <summary>
    /// 定义一个游戏规则容器，可以按照规则唯一标识获取规则数据
    /// </summary>
    protected class GameRuleDataItemCollection : KeyedCollection<Guid, GameRuleItem>
    {
      protected override Guid GetKeyForItem( GameRuleItem item )
      {
        return item.Guid;
      }
    }


    /// <summary>
    /// 创建 ActionConstraint 对象
    /// </summary>
    /// <param name="data">限制数据</param>
    /// <returns></returns>
    public abstract ActionConstraint CreateConstraint( JObject data );

    /// <summary>
    /// 初始化游戏规则
    /// </summary>
    /// <param name="typeResolver"></param>
    public abstract void Initialize();



    public abstract GameRuleItem GetDataItem( Guid id );

    public virtual GameRuleItem GetDataItem( Guid? id )
    {
      if ( id == null )
        return null;

      else
        return GetDataItem( id.Value );
    }



    public virtual T GetDataItem<T>( Guid? id ) where T : GameRuleItem
    {
      if ( id == null )
        return null;

      var result = GetDataItem( id.Value ) as T;
      if ( result == null )
        throw new InvalidDataException( "游戏数据注册类型错误 ID: " + id );

      return result;
    }




    /// <summary>
    /// 获取玩家初始点
    /// </summary>
    /// <param name="dataService">数据服务</param>
    /// <returns></returns>
    public abstract Coordinate GetInitiation();


    /// <summary>
    /// 初始化玩家对象
    /// </summary>
    /// <param name="player"></param>
    public abstract void InitializePlayer( GamePlayer player );


    /// <summary>
    /// 创建一个指定坐标的 Place 对象
    /// </summary>
    /// <param name="coordinate">地块坐标</param>
    /// <returns>指定坐标的 Place 对象</returns>
    public abstract Place CreatePlace( Coordinate coordinate );


    /// <summary>
    /// 根据名称获取类型
    /// </summary>
    /// <param name="typeName">类型名称</param>
    /// <returns>类型实例</returns>

    public abstract Type GetType( string typeName );


  }
}
