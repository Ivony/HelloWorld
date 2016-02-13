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
  public class ImmovableDescriptor : GameRuleItem
  {



    protected override void Initialize( GameRulesBase rules, JObject data )
    {
      base.Initialize( rules, data );

      Name = data.Value<string>( "Name" );
      Description = data.Value<string>( "Description" );

      InstanceType = rules.GetType( (string) data["InstanceType"], DefaultInstanceType );
    }



    /// <summary>
    /// 如果没有设置 InstanceType 则需要使用的默认类型
    /// </summary>
    protected virtual Type DefaultInstanceType
    {
      get { return typeof( Immovable ); }
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
    /// 获取 Building 对象的类型
    /// </summary>
    public Type InstanceType
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


      var type = InstanceType;
      var building = (Building) Activator.CreateInstance( type );

      building.InitializeData( place.DataService, data );
      return building;
    }
  }
}