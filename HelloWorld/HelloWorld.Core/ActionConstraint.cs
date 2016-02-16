using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{

  /// <summary>
  /// 代表一个活动的限制条件
  /// </summary>
  public class ActionConstraint
  {


    private JObject _data;


    public ActionConstraint( GameRulesBase rules, JObject data )
    {
      _data = data;

      BuildingDescriptors = new HashSet<Guid>();
      TerrainDescriptors = new HashSet<Guid>();
      UnitDescriptors = new HashSet<Guid>();


      {
        var constraint = _data["Building"] as JObject;
        if ( constraint != null )
        {
          var descriptors = constraint["Descriptors"] as JArray ?? new JArray();
          foreach ( JValue item in descriptors )
          {
            var guid = item.GuidValue();
            if ( guid.HasValue )
              BuildingDescriptors.Add( guid.Value );
          }

          var typeName = (string) constraint["Type"];
          BuildingType = rules.GetType( typeName );
        }
      }


      {
        var constraint = _data["Terrain"] as JObject;
        if ( constraint != null )
        {
          var descriptors = constraint["Descriptors"] as JArray ?? new JArray();
          foreach ( JValue item in descriptors )
          {
            var guid = item.GuidValue();
            if ( guid.HasValue )
              TerrainDescriptors.Add( guid.Value );
          }

          var typeName = (string) constraint["Type"];
          TerrainType = rules.GetType( typeName );
        }
      }

    }



    /// <summary>
    /// 限制数据
    /// </summary>
    public JObject ConstraintData { get { return (JObject) _data.DeepClone(); } }


    /// <summary>
    /// 是否限制在自己领土范围内
    /// </summary>
    public bool InTerritory { get; private set; }

    /// <summary>
    /// 必须存在指定建筑
    /// </summary>
    public HashSet<Guid> BuildingDescriptors { get; private set; }

    /// <summary>
    /// 必须在指定地形
    /// </summary>
    public HashSet<Guid> TerrainDescriptors { get; private set; }

    /// <summary>
    /// 必须存在指定单位
    /// </summary>
    public HashSet<Guid> UnitDescriptors { get; private set; }

    /// <summary>
    /// 建筑类型限制条件
    /// </summary>
    public Type BuildingType { get; private set; }

    /// <summary>
    /// 地形类型限制条件
    /// </summary>
    public Type TerrainType { get; private set; }

    /// <summary>
    /// 单位类型限制条件
    /// </summary>
    public Type UnitType { get; private set; }





    /// <summary>
    /// 检查指定地块当前是否满足限制。
    /// </summary>
    /// <param name="player">当前操作的玩家</param>
    /// <param name="place">要检查的地块</param>
    /// <returns>是否满足限制</returns>
    public virtual bool IsSatisfy( GamePlayer player, PlaceBase place )
    {

      return Check( player, place ) == null;


    }


    /// <summary>
    /// 检查指定地块当前是否满足限制。
    /// </summary>
    /// <param name="player">当前操作的玩家</param>
    /// <param name="place">要检查的地块</param>
    /// <param name="errorMessage">错误信息</param>
    /// <returns>是否满足限制</returns>
    public virtual bool IsSatisfy( GamePlayer player, PlaceBase place, out string errorMessage )
    {

      errorMessage = Check( player, place );
      return errorMessage == null;

    }



    /// <summary>
    /// 检查指定地块当前是否满足限制。
    /// </summary>
    /// <param name="player">当前操作的玩家</param>
    /// <param name="place">要检查的地块</param>
    /// <returns>若不满足限制，返回原因，若满足限制，则返回null</returns>
    protected virtual string Check( GamePlayer player, PlaceBase place )
    {

      var placeInstance = place as Place;
      if ( placeInstance != null )
        return Check( player, placeInstance );

      else
        return null;//默认允许通过
    }



    /// <summary>
    /// 检查指定地块当前是否满足限制。
    /// </summary>
    /// <param name="player">当前操作的玩家</param>
    /// <param name="place">要检查的地块</param>
    /// <returns>若不满足限制，返回原因，若满足限制，则返回null</returns>
    protected virtual string Check( GamePlayer player, Place place )
    {

      if ( player == null )
        throw new ArgumentNullException( "player" );

      if ( place == null )
        throw new ArgumentNullException( "place" );



      if ( InTerritory && place.Owner == player.Guid )
        return "只能在自己的领土上上开展该活动";



      if ( TerrainType != null )
      {
        if ( place.Terrain == null || TerrainType.IsAssignableFrom( place.Terrain.GetType() ) == false )
          return "地形不满足活动需求";
      }

      if ( TerrainDescriptors.Any() )
      {
        if ( place.Terrain == null || TerrainDescriptors.Contains( place.Terrain.Descriptor.Guid ) == false )
          return "地形不满足活动需求";
      }



      if ( BuildingType != null )
      {
        if ( place.Terrain == null || BuildingType.IsAssignableFrom( place.Building.GetType() ) == false )
          return "地形不满足活动需求";
      }

      if ( BuildingDescriptors.Any() )
      {
        if ( place.Terrain == null || BuildingDescriptors.Contains( place.Building.Descriptor.Guid ) == false )
          return "地形不满足活动需求";
      }



      if ( UnitType != null )
      {
        if ( place.GetUnits().All( unit => UnitType.IsAssignableFrom( unit.GetType() ) == false ) )
          return "单位不满足活动需求";
      }

      if ( UnitDescriptors.Any() )
      {
        if ( place.GetUnits().All( unit => UnitDescriptors.Contains( unit.Descriptor.Guid ) == false ) )
          return "单位不满足活动需求";
      }


      return null;
    }



    public override string ToString()
    {
      return ConstraintData.ToString();
    }


  }
}
