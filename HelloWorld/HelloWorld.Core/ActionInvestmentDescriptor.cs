using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{



  /// <summary>
  /// 定义游戏活动需要投入的资源描述
  /// </summary>
  public class ActionInvestmentDescriptor
  {





    public static ActionInvestmentDescriptor FromData( JObject data )
    {

      if ( data == null )
        return null;

      return new ActionInvestmentDescriptor
      {
        Items = ItemListJsonConverter.FromJson( (JObject) data["Items"] ),
        Time = data.TimeValue( "Time" ),
      };
    }


    public ItemList Items { get; private set; }

    public TimeSpan Time { get; private set; }




    /// <summary>
    /// 在指定的地块尝试进行投入
    /// </summary>
    /// <param name="place">要进行投入的地块</param>
    /// <returns>是否成功扣除所需物品</returns>
    public bool TryInvest( Place place )
    {
      lock ( place.SyncRoot )
      {
        return place.Owner.Resources.RemoveItems( Items );
      }
    }
  }
}
