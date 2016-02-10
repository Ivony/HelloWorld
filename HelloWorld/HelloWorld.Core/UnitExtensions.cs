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

    public static Unit NewUnit( this Place place, Guid unitDescriptor )
    {
      var dataService = place.DataService;
      return Unit.CreateUnit( dataService, GameHost.GameRules.GetDataItem<UnitDescriptor>( new Guid( "{72213162-0D16-4C53-89F3-AE2A0180E031}" ) ), place.Owner.Value, place.Coordinate, Guid.NewGuid(), dataService.NameService.AllocateName() );
    }

  }
}
