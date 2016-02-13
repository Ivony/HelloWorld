using System;

namespace HelloWorld
{

  /// <summary>
  /// 定义 Place 对象抽象
  /// </summary>
  public abstract class PlaceBase : GameDataItem
  {
    /// 地块坐标
    /// </summary>
    public Coordinate Coordinate { get; private set; }



    /// <summary>
    /// 创建 Place 对象
    /// </summary>
    /// <param name="coordinate">地块所处坐标</param>
    public PlaceBase( Coordinate coordinate )
    {
      Coordinate = coordinate;
    }



    /// <summary>
    /// 获取可以进行的操作列表
    /// </summary>
    /// <returns>可以进行的操作列表</returns>
    public abstract ActionDescriptor[] GetActions();



    /// <summary>
    /// 对地块进行例行检查
    /// </summary>
    /// <param name="now">系统时间</param>
    public abstract void Check( DateTime now );

  }
}