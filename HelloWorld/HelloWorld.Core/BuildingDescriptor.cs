using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 建筑
  /// </summary>
  [Guid( "D681EF16-D0D2-4B72-834B-5ADDB0867535" )]
  public class BuildingDescriptor : GameRuleDataItem
  {


    private BuildingDescriptor( Guid guid, JObject data ) : base( guid, data ) { }

    protected BuildingDescriptor( BuildingDescriptor building )
      : this( building.Guid, building.Data )
    {
      Name = building.Name;
      Description = building.Description;
    }


    /// <summary>
    /// 从数据中加载 BuildingDescriptor 对象
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static BuildingDescriptor FromData( Guid guid, JObject data )
    {
      if ( data == null )
        return null;

      return new BuildingDescriptor( guid, data )
      {
        Name = data.Value<string>( "Name" ),
        Description = data.Value<string>( "Description" ),
      };
    }


    /// <summary>
    /// 建筑名
    /// </summary>
    public string Name
    {
      get;
      private set;
    }

    /// <summary>
    /// 建筑描述
    /// </summary>
    public string Description
    {
      get;
      private set;
    }


    /// <summary>
    /// 系统定期调用此方法通知建筑物检查自己的状态，或者执行相应的事件
    /// </summary>
    /// <param name="place">建筑物所在的地块</param>
    public virtual void Check( Place place )
    {

      place.CheckPoint = DateTime.UtcNow;

    }

  }
}