using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 提供随机数产生和概率计算的类型
  /// </summary>
  public static class Probability
  {

    private static Random random = new Random();

    private static object sync = new object();


    /// <summary>
    /// 获取一个随机的数值
    /// </summary>
    /// <param name="maxValue">最大值</param>
    /// <returns></returns>
    public static int RandomNumber( int maxValue )
    {
      lock ( sync )
        return random.Next( maxValue );
    }


    /// <summary>
    /// 如果以指定概率计算，确定是否命中
    /// </summary>
    /// <param name="hitProbability">命中概率</param>
    /// <returns>是否命中</returns>
    public static bool HasHit( double hitProbability )
    {
      lock ( sync )
        return random.NextDouble() < hitProbability;
    }


    /// <summary>
    /// 如果以指定的概率计算，若命中，则执行指定方法
    /// </summary>
    /// <param name="hitProbability">命中概率</param>
    /// <param name="action">命中后要执行的方法</param>
    /// <returns>是否命中</returns>
    public static bool IfHit( double hitProbability, Action action )
    {
      if ( HasHit( hitProbability ) )
      {
        action();
        return true;
      }

      else
        return false;

    }

  }
}
