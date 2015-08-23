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
  public class GameActingInvestmentDescriptor
  {





    public static GameActingInvestmentDescriptor FromData( JObject data )
    {

      if ( data == null )
        return null;

      return new GameActingInvestmentDescriptor
      {
        Items = ItemListJsonConverter.FromJson( (JObject) data["Items"] ),
        Time = data.TimeValue( "Time" ),
        Workers = data.Value<int>( "Workers" ),
      };
    }


    public ItemList Items { get; private set; }

    public TimeSpan Time { get; private set; }

    public int Workers { get; private set; }




    /// <summary>
    /// 在指定的地块尝试进行投入
    /// </summary>
    /// <param name="place">要进行投入的地块</param>
    /// <returns>是否成功扣除所需物品</returns>
    public bool TryInvest( Place place )
    {

      lock ( place.SyncRoot )
      {
        if ( place.Owner.Workers < Workers )
          return false;

        if ( place.Owner.Resources.RemoveItems( Items ) == false )
          return false;

        place.Owner.Workers -= Workers;
        return true;


      }

    }
  }
}
