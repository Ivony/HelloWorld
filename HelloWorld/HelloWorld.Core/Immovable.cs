using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 不可移动的对象基类
  /// </summary>
  public class Immovable : GameDataItem
  {


    /// <summary>
    /// 初始化数据
    /// </summary>
    protected override void Initialize()
    {
      Descriptor = GameHost.GameRules.GetDataItem<ImmovableDescriptor>( JsonObject.GuidValue( "Descriptor" ) );

      base.Initialize();

    }


    /// <summary>
    /// 不可移动对象描述对象
    /// </summary>
    public ImmovableDescriptor Descriptor { get; private set; }


    /// <summary>
    /// 初始化建筑数据
    /// </summary>
    /// <param name="data">数据对象</param>
    internal virtual void InitializeData( PlaceBase place, JsonDataObject data )
    {
      Place = place;
      InitializeData( place.DataService, place, data );
    }

    /// <summary>
    /// 建筑物所在地块
    /// </summary>
    public PlaceBase Place { get; private set; }


    /// <summary>
    /// 检查建筑物是否满足建筑物限制
    /// </summary>
    /// <param name="constraint"></param>
    /// <returns></returns>
    public bool IsSatisfy( ActionConstraint constraint )
    {
      return true;
    }


    /// <summary>
    /// 进行例行检查
    /// </summary>
    /// <param name="now"></param>
    public virtual void Check( DateTime now ) { }


    public virtual object GetInfo()
    {
      return Descriptor.GetInfo();
    }

  }
}
