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
  }
}