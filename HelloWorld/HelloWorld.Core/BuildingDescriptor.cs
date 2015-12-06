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




    private static object _sync = new object();

    private HashSet<ActionDescriptor> _actions = new HashSet<ActionDescriptor>();

    internal void RegisterAction( ActionDescriptor action )
    {

      lock ( _sync )
      {
        _actions.Add( action );
      }

    }


    /// <summary>
    /// 获取建筑可以执行的行动列表
    /// </summary>
    /// <returns>可以执行的行动列表</returns>
    public virtual ActionDescriptor[] GetActions()
    {

      return _actions.ToArray();

    }


  }
}