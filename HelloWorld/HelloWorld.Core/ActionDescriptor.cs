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


    protected override void Initialize( GameRulesBase rules, JObject data )
    {
      base.Initialize( rules, data );

      Constraint = rules.CreateConstraint( (JObject) data["Constraint"] );
      Requirement = ActionInvestmentDescriptor.FromData( (JObject) data["Requirement"] );
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
    public ActionConstraint Constraint { get; private set; }


    /// <summary>
    /// 生产所需资源列表
    /// </summary>
    public ActionInvestmentDescriptor Requirement { get; private set; }


    /// <summary>
    /// 生产结束后的回报
    /// </summary>
    public ActionReturnsDescriptor Returns { get; private set; }


    /// <summary>
    /// 确认是否可以在指定地块开始这个活动
    /// </summary>
    /// <param name="place">地块对象</param>
    /// <returns>是否可以开展这个活动</returns>
    public override sealed bool CanStartAt( GamePlayer player, PlaceBase place )
    {
      var instance = place as Place;

      if ( instance == null )
        return false; ;


      return Constraint.IsSatisfy( player, place );
    }






    /// <summary>
    /// 尝试在指定地块开始这个活动 
    /// </summary>
    /// <param name="place">要开始活动的地方</param>
    /// <returns>正在进行的活动</returns>
    public override PlaceActing TryStartAt( GamePlayer player, PlaceBase place )
    {


      if ( player == null )
        throw new ArgumentNullException( "player" );

      if ( place == null )
        throw new ArgumentNullException( "place" );



      string errorMessage;
      if ( Constraint.IsSatisfy( player, place, out errorMessage ) == false )
        throw new InvalidOperationException( errorMessage );



      var instance = place as Place;
      if ( instance == null )
        return null;

      return TryStartAt( player, instance );

    }


    /// <summary>
    /// 尝试在指定地块开始这个活动 
    /// </summary>
    /// <param name="place">要开始活动的地方</param>
    /// <returns>正在进行的活动</returns>
    protected virtual PlaceActing TryStartAt( GamePlayer player, Place place )
    {


      lock ( place )
      {
        if ( place.Acting != null )
          throw new InvalidOperationException( "土地上已经存在一个正在进行的活动" );

        if ( Requirement.TryInvest( place ) == false )
          return null;
      }


      return PlaceActing.StartAt( player, place, this );

    }



    /// <summary>
    /// 检查活动是否已经完成，若已经完成则获取相应的收益
    /// </summary>
    /// <param name="acting">正在进行的活动</param>
    /// <returns>活动是否已经完成</returns>
    public override bool TryComplete( PlaceActing acting, DateTime now )
    {
      var completedOn = acting.StartOn + Requirement.Time;

      if ( completedOn > now )
        return false;

      var place = acting.Place;
      var player = acting.GetPlayer();


      if ( Returns.Items != null && player != null )
        player.Resources.AddItems( Returns.Items );

      if ( Returns.Building != null )
        place.SetBuilding( Returns.Building.Guid );


      place.CheckPoint = completedOn;

      if ( player != null )
      {
        var message = new GameMessageEntry( completedOn, string.Format( "通过不懈的努力，在位置 {0} 的活动 {1} 已经完成 {2}", place.Coordinate.ToRelative( player ), Name, Returns.DescriptiveMessage ) );
        GameHost.MessageService.AddMessage( player.Guid, message );
      }
      return true;
    }

    public override object GetInfo()
    {
      return new
      {
        Guid,
        Name,
        Description,
        Requirement,
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

        if ( Requirement.Items != null && Requirement.Items.Any() )
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
