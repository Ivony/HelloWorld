using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 定义为游戏提供数据的服务
  /// </summary>
  public interface IGameDataService
  {
    /// <summary>
    /// 获取游戏地块对象
    /// </summary>
    /// <param name="coordinate">地块坐标</param>
    /// <returns>游戏地块对象</returns>
    Place GetPlace( Coordinate coordinate );

    /// <summary>
    /// 获取游戏玩家对象
    /// </summary>
    /// <param name="userId">玩家 ID</param>
    /// <returns></returns>
    GamePlayer GetPlayer( Guid userId );



    /// <summary>
    /// 获取地块上所有的单位
    /// </summary>
    /// <param name="coordinate">地块坐标</param>
    /// <returns></returns>
    Unit[] GetUnits( Coordinate coordinate );

    /// <summary>
    /// 获取玩家所有单位
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    Unit[] GetUnits( GamePlayer player );



    /// <summary>
    /// 保存游戏数据对象
    /// </summary>
    /// <param name="dataItem"></param>
    void Save( GameDataItem dataItem );


    /// <summary>
    /// 初始化游戏数据服务
    /// </summary>
    void Initialize();


  }


}
