using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

  /// <summary>
  /// 根据名称获取实现类型
  /// </summary>
  public interface ITypeResolver
  {

    Type GetType( string typeName );

  }

  internal sealed class TypeResolver : ITypeResolver
  {
    Type ITypeResolver.GetType( string typeName )
    {
      return Type.GetType( typeName );
    }
  }
}
