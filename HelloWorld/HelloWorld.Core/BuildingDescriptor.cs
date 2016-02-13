using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 建筑描述符
  /// </summary>
  [Guid( "D681EF16-D0D2-4B72-834B-5ADDB0867535" )]
  public class BuildingDescriptor : GameRuleItem
  {



    protected override void Initialize( JObject data )
    {
      base.Initialize( data );

      Name = data.Value<string>( "Name" );
      Description = data.Value<string>( "Description" );

      BuildingType = GameHost.GameRules.ParseType( (string) data["BuildingType"], typeof( Building ) );
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
    public virtual void Check( Place place, DateTime now )
    {

      place.CheckPoint = now;

    }


    /// <summary>
    /// 获取 Building 对象的类型
    /// </summary>
    public Type BuildingType
    {
      get;
      private set;
    }


    /// <summary>
    /// 创建一个 Building 对象
    /// </summary>
    /// <param name="place">所处地块</param>
    /// <returns>Building 对象</returns>
    public Building CreateBuilding( Place place )
    {
      var data = new JObject() as dynamic;

      data.Descriptor = this.Guid;


      var type = BuildingType;
      var building = (Building) Activator.CreateInstance( type );

      building.InitializeData( place.DataService, data );
      return building;
    }
  }
}