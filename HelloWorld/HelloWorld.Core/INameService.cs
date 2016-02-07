namespace HelloWorld
{

  /// <summary>
  /// 名称服务
  /// </summary>
  public interface INameService
  {
    /// <summary>
    /// 分配一个名称
    /// </summary>
    /// <returns>要分配的名称</returns>
    string AllocateName();


    /// <summary>
    /// 释放一个名称
    /// </summary>
    /// <param name="name">要释放的名称</param>
    /// <returns></returns>
    bool ReleaseName( string name );


    /// <summary>
    /// 校验一个名称是否合法
    /// </summary>
    /// <param name="name">要校验的名称</param>
    /// <returns></returns>
    bool ValidateName( string name );
  }
}