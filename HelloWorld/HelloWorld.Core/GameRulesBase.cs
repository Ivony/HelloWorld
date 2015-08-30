using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  public abstract class GameRulesBase
  {


    /// <summary>
    /// 定义一个游戏规则容器，可以按照规则唯一标识获取规则数据
    /// </summary>
    protected class GameRuleDataItemCollection : KeyedCollection<Guid, GameRuleDataItem>
    {
      protected override Guid GetKeyForItem( GameRuleDataItem item )
      {
        return item.Guid;
      }
    }

    /// <summary>
    /// 初始化游戏环境
    /// </summary>
    /// <param name="typeResolver"></param>
    public abstract void Initialize();



    public abstract GameRuleDataItem GetDataItem( Guid id );



    public virtual T GetDataItem<T>( Guid id ) where T : GameRuleDataItem
    {

      var result = GetDataItem( id ) as T;
      if ( result == null )
        throw new InvalidDataException( "游戏数据注册类型错误 ID: " + id );

      return result;
    }



    /// <summary>
    /// 获取某个地块可以进行的活动
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public abstract ActionDescriptor[] GetActions( Place place );



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
