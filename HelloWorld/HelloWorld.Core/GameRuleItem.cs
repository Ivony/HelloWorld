using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 游戏规则对象的基础类型
  /// </summary>
  public abstract class GameRuleItem
  {



    /// <summary>
    /// 游戏规则的唯一标识
    /// </summary>
    public Guid Guid { get; private set; }



    /// <summary>
    /// 创建游戏规则对象
    /// </summary>
    protected GameRuleItem()
    {

    }


    /// <summary>
    /// 初始化游戏规则数据
    /// </summary>
    /// <param name="data">游戏规则数据</param>
    protected virtual void Initialize( JObject data )
    {

      if ( data == null )
        throw new ArgumentNullException( "data" );


      Guid = (Guid) data.GuidValue( "ID" );
      Data = (JObject) data.DeepClone();
    }



    /// <summary>
    /// 游戏规则数据
    /// </summary>
    protected JObject Data { get; private set; }

    /// <summary>
    /// 重写 ToString 方法，输出 JSON 格式的数据
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return Data.ToString(); }


    /// <summary>
    /// 重写 Equals 方法，判断规则 ID 是否一致
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object obj )
    {
      var item = obj as GameRuleItem;

      if ( item == null )
        return false;

      return Guid.Equals( item.Guid );
    }

    public override int GetHashCode()
    {
      return Guid.GetHashCode();
    }


    public static bool operator ==( GameRuleItem a, GameRuleItem b )
    {

      if ( object.ReferenceEquals( a, null ) && object.ReferenceEquals( b, null ) )
        return true;

      if ( object.ReferenceEquals( a, null ) || object.ReferenceEquals( b, null ) )
        return false;

      return a.Equals( b );
    }


    public static bool operator !=( GameRuleItem a, GameRuleItem b )
    {

      if ( object.ReferenceEquals( a, null ) && object.ReferenceEquals( b, null ) )
        return false;

      if ( object.ReferenceEquals( a, null ) || object.ReferenceEquals( b, null ) )
        return true;

      return a.Equals( b ) == false;
    }


    public virtual object GetInfo()
    {
      return this;
    }




    /// <summary>
    /// 标识对象是否已经被初始化完毕
    /// </summary>
    protected bool AlreadyInitialized { get; private set; }


    internal void InitializeCore( JObject data )
    {
      Initialize( data );

      AlreadyInitialized = true;
    }
  }
}
