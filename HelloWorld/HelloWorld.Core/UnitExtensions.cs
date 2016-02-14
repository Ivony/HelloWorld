using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 关于玩家单位的一些扩展方法
  /// </summary>
  public static class UnitExtensions
  {

    public static Unit NewUnit( this Place place, Guid descriptorId, Guid? playerId = null )
    {

      var descriptor = GameHost.GameRules.GetDataItem<UnitDescriptor>( descriptorId );
      var owner = playerId ?? place.Owner ?? Guid.Empty;

      if ( owner == Guid.Empty )
        throw new ArgumentNullException( "playerId", "无法从地块找到单位所有者，必须指定单位所有玩家" );


      return Unit.CreateUnit( place.DataService, descriptor, owner, place.Coordinate, Guid.NewGuid(), place.DataService.NameService.AllocateName() );
    }

  }
}
