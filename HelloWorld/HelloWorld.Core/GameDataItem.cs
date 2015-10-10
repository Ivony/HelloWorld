using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  public abstract class GameDataItem : IDataHost
  {

    protected GameDataItem( IGameDataService dataService )
    {
      DataService = dataService;
      SyncRoot = new object();

      _data = new JsonDataObject( this );
    }







    internal void InitializeData( JObject data )
    {
      JsonObject.Merge( data );
      Initialize();
    }



    /// <summary>
    /// 用于同步的对象
    /// </summary>
    public object SyncRoot { get; private set; }



    protected virtual void Initialize()
    {
    }



    /// <summary>
    /// 持久化数据的服务
    /// </summary>
    public IGameDataService DataService { get; private set; }



    private JsonDataObject _data;

    /// <summary>
    /// 存放数据的动态对象
    /// </summary>
    protected dynamic DataObject { get { return _data; } }

    /// <summary>
    /// 存放数据的 JSON 对象
    /// </summary>
    protected JObject JsonObject { get { return _data; } }


    /// <summary>
    /// 通知数据服务保存数据对象
    /// </summary>
    protected virtual void Save()
    {
      DataService.Save( this );
    }


    void IDataHost.Save( JToken dataItem )
    {
      Save();
    }


    private Guid _token = Guid.NewGuid();
    Guid IDataHost.Token { get { return _token; } }

    internal string SaveAsJson()
    {
      return JsonObject.ToString( Newtonsoft.Json.Formatting.None );
    }

  }


  /// <summary>
  /// 定义游戏数据类别枚举
  /// </summary>
  public enum GameDataType
  {
    Place,
    Player,
    Unit,
    Building,
    Acting,
  }

}
