using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 定义一个实际存在的建筑
  /// </summary>
  public class Building : GameDataItem
  {


    public Building() { }


    /// <summary>
    /// 初始化数据
    /// </summary>
    protected override void Initialize()
    {
      BuildingDescriptor = GameHost.GameRules.GetDataItem<BuildingDescriptor>( JsonObject.GuidValue( "Descriptor" ) );

      base.Initialize();

    }


    /// <summary>
    /// 建筑物描述对象
    /// </summary>
    public BuildingDescriptor BuildingDescriptor { get; private set; }


    /// <summary>
    /// 初始化建筑数据
    /// </summary>
    /// <param name="data">数据对象</param>
    internal void InitializeData( Place place, JsonDataObject data )
    {
      Place = place;
      InitializeData( place.DataService, place, data );
    }

    /// <summary>
    /// 建筑物所在地块
    /// </summary>
    public Place Place { get; private set; }


    /// <summary>
    /// 检查建筑物是否满足建筑物限制
    /// </summary>
    /// <param name="restriction"></param>
    /// <returns></returns>
    public bool IsSatisfy( ActionRestriction restriction )
    {
      return BuildingDescriptor == restriction.Building;
    }

    public virtual void Check( DateTime now )
    {
      BuildingDescriptor.Check( Place, now );
    }

    public virtual object GetInfo()
    {
      return BuildingDescriptor.GetInfo();
    }
  }
}
