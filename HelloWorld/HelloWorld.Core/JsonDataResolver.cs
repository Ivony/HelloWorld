using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  /// <summary>
  /// 定义一个 JSON 数据读取处理程序
  /// </summary>
  public interface IJsonDataResolver
  {

    /// <summary>
    /// 加载指定名称的数据对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    JObject LoadDataObject( string name );

    /// <summary>
    /// 加载所有数据对象
    /// </summary>
    /// <returns></returns>
    IEnumerable<JObject> LoadAllData();

  }


  /// <summary>
  /// IJsonDataResolver 的一个简单实现
  /// </summary>
  public class JsonDataResolver : IJsonDataResolver
  {

    public JsonDataResolver() : this( ConfigurationManager.AppSettings["GamePath"] ) { }

    public JsonDataResolver( string dataRoot )
    {

      _dataRoot = dataRoot;

    }


    private string _dataRoot;



    public JObject LoadDataObject( string name )
    {
      var filepath = Path.Combine( _dataRoot, name );

      return LoadData( filepath ) ?? LoadData( filepath + ".json" );
    }

    public IEnumerable<JObject> LoadAllData()
    {

      var files = Directory.EnumerateFiles( _dataRoot, "*", SearchOption.AllDirectories );
      return files.Select( f => LoadData( f ) ).Where( data => data != null );

    }

    private JObject LoadData( string filepath )
    {
      if ( File.Exists( filepath ) )
        return JObject.Parse( File.ReadAllText( filepath ) );

      else
        return null;
    }
  }


}
