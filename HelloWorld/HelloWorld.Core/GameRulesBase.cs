using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// 初始化游戏环境
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
    /// 获取初始建筑
    /// </summary>
    public abstract BuildingDescriptor InitiationBuilding { get; }



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


  }
}
