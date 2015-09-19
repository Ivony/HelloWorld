using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace HelloWorld
{



  /// <summary>
  /// 定义游戏活动需要投入的资源描述
  /// </summary>
  public class ActionReturnsDescriptor
  {





    public static ActionReturnsDescriptor FromData( JObject data )
    {

      if ( data == null )
        return null;

      return new ActionReturnsDescriptor
      {
        Items = ItemListJsonConverter.FromJson( (JObject) data["Items"] ),
        Building = GameHost.GameRules.GetDataItem<BuildingDescriptor>( data.GuidValue( "Building" ) ),
      };
    }


    public ItemList Items { get; private set; }

    public BuildingDescriptor Building { get; set; }


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



    /// <summary>
    /// 转换为游戏消息中的描述信息
    /// </summary>
    /// <returns></returns>
    public string DescriptiveMessage
    {
      get
      {
        var builder = new StringBuilder();


        if ( Building != null )
        {
          builder.AppendFormat( "您获得了新的建筑 {0} ", Building.Name );

          if ( Items.Any() )
            builder.AppendFormat( "以及 {0} ", GetDescriptiveMessage( Items ) );
        }
        else
        {
          builder.AppendFormat( "您获得了 {0} ", GetDescriptiveMessage( Items ) );
        }


        builder.Append( "。" );
        return builder.ToString();


      }
    }

    private static string GetDescriptiveMessage( ItemList items )
    {
      return string.Join( "、", items.Select( i => string.Format( "{0}*{1}", i.ItemDescriptor.Name, i.Quantity ) ) );
    }
  }
}
