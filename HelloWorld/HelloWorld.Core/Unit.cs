using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{


  /// <summary>
  /// 代表一个玩家的单位
  /// </summary>
  public class Unit
  {

    public UnitDescriptor UnitDescriptor { get; private set; }


    public Place Place { get; private set; }


    public UnitActionState ActionState { get; protected set; }


    public ActionDescriptor[] GetActions()
    {

      return UnitDescriptor.GetActions( this );

    }


  }



  /// <summary>
  /// 定义单位的行动状态
  /// </summary>
  public enum UnitActionState
  {
    /// <summary>空闲</summary>
    Idle,
    /// <summary>休息</summary>
    Rest,
    /// <summary>正在移动</summary>
    Moving,
    /// <summary>正在执行某任务</summary>
    Acting
  }



}
