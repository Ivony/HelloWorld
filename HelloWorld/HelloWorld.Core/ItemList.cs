using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Core
{

  /// <summary>
  /// 定义一个物品清单
  /// </summary>
  public sealed class ItemList : IReadOnlyCollection<Item>
  {
    public int Count
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public IEnumerator<Item> GetEnumerator()
    {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }
  }
}
