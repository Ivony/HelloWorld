using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Dispatcher;

namespace HelloWorld.WebHost
{
  public sealed class HttpRuntimeTypeResolver : ITypeResolver
  {
    private Assembly[] assemblies;

    public HttpRuntimeTypeResolver( IAssembliesResolver assembliesResolver )
    {
      assemblies = assembliesResolver.GetAssemblies().ToArray();

    }

    public Type GetType( string typeName )
    {
      var types = assemblies.Select( a => a.GetType( typeName ) ).Where( type => type != null ).Take( 2 ).ToArray();

      if ( types.Length == 0 )
        return null;


      if ( types.Length > 1 )
        throw new InvalidOperationException( string.Format( "类型 {0} 同时存在于程序集 {1} 和 {2} 中", typeName, types[0].Assembly.FullName, types[1].Assembly.FullName ) );

      return types[0];


    }
  }
}