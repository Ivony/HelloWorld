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


    private ActionDescriptor( Guid guid, JObject data ) : base( guid, data ) { }


    public static ActionDescriptor FromData( Guid guid, JObject data )
    {

      if ( data == null )
        return null;

      var instance = new ActionDescriptor( guid, data )
      {
        Name = data.Value<string>( "Name" ),
        Description = data.Value<string>( "Description" ),
        Building = GameHost.GameRules.GetDataItem<BuildingDescriptor>( data.GuidValue( "Building" ) ),
        Requirment = ActionInvestmentDescriptor.FromData( (JObject) data["Requirment"] ),
        Returns = ActionReturnsDescriptor.FromData( (JObject) data["Returns"] ),
      };


      if ( instance.Name == null )
        instance.Name = instance.DefaultName();


      if ( instance.Description == null )
        instance.Description = instance.DefaultDescription();


      return instance;

    }

    public string Name { get; private set; }


    public string Description { get; private set; }


    /// <summary>
    /// 此生产过程所依赖的建筑/场所
    /// </summary>
    public BuildingDescriptor Building { get; private set; }



    /// <summary>
    /// 生产所需资源列表
    /// </summary>
    public ActionInvestmentDescriptor Requirment { get; private set; }


    /// <summary>
    /// 生产结束后的回报
    /// </summary>
    public ActionReturnsDescriptor Returns { get; private set; }


    /// <summary>
    /// 尝试在指定地块开始这个活动 
    /// </summary>
    /// <param name="place">要开始活动的地方</param>
    /// <returns>正在进行的活动</returns>
    public override GameActing TryStartAt( Place place )
    {
      if ( place == null )
        throw new ArgumentNullException( "place" );

      var acting = new GameActing( this );

      lock ( place )
      {
        if ( place.Acting != null )
          throw new InvalidOperationException();

        if ( Requirment.TryInvest( place ) == false )
          return null;

        acting.StartAt( place );
      }

      return acting;
    }


    /// <summary>
    /// 检查活动是否已经完成，若已经完成则获取相应的收益
    /// </summary>
    /// <param name="acting">正在进行的活动</param>
    /// <returns>活动是否已经完成</returns>
    public override bool TryComplete( GameActing acting )
    {

      if ( acting.StartOn + Requirment.Time > DateTime.UtcNow )
        return false;

      if ( Returns.Items != null )
        acting.Place.Owner.Resources.AddItems( Returns.Items );

      if ( Returns.Building != null )
        acting.Place.Building = Returns.Building;

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
