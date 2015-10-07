using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 游戏活动过程的具体实现
  /// </summary>
  [Guid( "AA393552-5020-4400-B565-FA613AF7A123" )]
  public class ActionDescriptor : ActionDescriptorBase
  {


    protected override void Initialize( JObject data )
    {
      base.Initialize( data );

      BuildingRestriction = BuildingRestriction.FromData( GameHost.GameRules, data["Building"] );
      Requirment = ActionInvestmentDescriptor.FromData( (JObject) data["Requirment"] );
      Returns = ActionReturnsDescriptor.FromData( (JObject) data["Returns"] );


      if ( data.Value<string>( "Name" ) == null )
        _name = DefaultName();
      else
        _name = data.Value<string>( "Name" );



      if ( data.Value<string>( "Description" ) == null )
        _description = DefaultDescription();
      else
        _description = data.Value<string>( "Description" );

    }



    private string _name;
    public override string Name { get { return _name; } }


    private string _description;
    public override string Description { get { return _description; } }


    /// <summary>
    /// 此生产过程所依赖的建筑/场所
    /// </summary>
    public BuildingRestriction BuildingRestriction { get; private set; }


    /// <summary>
    /// 此生产过程所依赖的单位
    /// </summary>
    public UnitRestriction UnitRestriction { get; private set; }

    /// <summary>
    /// 生产所需资源列表
    /// </summary>
    public ActionInvestmentDescriptor Requirment { get; private set; }


    /// <summary>
    /// 生产结束后的回报
    /// </summary>
    public ActionReturnsDescriptor Returns { get; private set; }


    public override bool CanStartAt( Place place )
    {
      return place.Building == BuildingRestriction.Building;
    }


    /// <summary>
    /// 尝试在指定地块开始这个活动 
    /// </summary>
    /// <param name="place">要开始活动的地方</param>
    /// <returns>正在进行的活动</returns>
    public override PlaceActing TryStartAt( Place place )
    {
      if ( place == null )
        throw new ArgumentNullException( "place" );


      if ( place.Owner == null )
        throw new InvalidOperationException( "不能在无主土地上开始活动" );

      if ( place.Acting != null )
        throw new InvalidOperationException( "土地上已经存在一个正在进行的活动" );

      if ( place.Building != BuildingRestriction.Building )
        throw new InvalidOperationException( "地块建筑不满足活动需求" );



      lock ( place )
      {
        if ( place.Acting != null )
          throw new InvalidOperationException();

        if ( Requirment.TryInvest( place ) == false )
          return null;
      }


      return PlaceActing.StartAt( place, this );
    }


    /// <summary>
    /// 检查活动是否已经完成，若已经完成则获取相应的收益
    /// </summary>
    /// <param name="acting">正在进行的活动</param>
    /// <returns>活动是否已经完成</returns>
    public override bool TryComplete( PlaceActing acting )
    {
      var completedOn = acting.StartOn + Requirment.Time;

      if ( completedOn > DateTime.UtcNow )
        return false;

      var place = acting.Place;
      var player = place.GetPlayer();

      if ( Returns.Items != null )
        player.Resources.AddItems( Returns.Items );

      if ( Returns.Building != null )
        place.Building = Returns.Building;


      place.CheckPoint = completedOn;
      var message = new GameMessageEntry( completedOn, string.Format( "通过不懈的努力，在位置 {0} 的活动 {1} 已经完成 {2}", place.GetPlayerCoordinate(), Name, Returns.DescriptiveMessage ) );
      GameHost.MessageService.AddMessage( player.UserID, message );
      return true;
    }

    public override object GetInfo()
    {
      return new
      {
        Guid,
        Name,
        Description,
        Requirment,
        Returns,
      };
    }




    protected virtual string DefaultName()
    {
      var builder = new StringBuilder();

      if ( Returns.Building != null )
        builder.AppendFormat( "建造 {0}", Returns.Building.Name );


      else if ( Returns.Items != null && Returns.Items.Any() )
      {

        if ( Requirment.Items != null && Requirment.Items.Any() )
          builder.Append( "生产 " );

        else
          builder.Append( "收获 " );


        builder.Append( string.Join( "、", Returns.Items.Select( i => i.ItemDescriptor.Name ) ) );
      }

      return builder.ToString();
    }




    protected virtual string DefaultDescription()
    {
      return DefaultName();
    }





  }
}
