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



  }
}
