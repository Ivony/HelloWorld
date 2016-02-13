using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace HelloWorld
{


  /// <summary>
  /// 定义游戏数据对象的抽象实现
  /// </summary>
  public abstract class GameDataItem : IDataHost
  {



    protected GameDataItem()
    {
      SyncRoot = new object();

      _data = new JsonDataObject( this );

    }



    /// <summary>
    /// 从 JSON 数据中初始化对象
    /// </summary>
    /// <param name="dataService">游戏数据服务</param>
    /// <param name="data">JSON 数据</param>
    internal void InitializeData( IGameDataService dataService, JObject data )
    {
      DataService = dataService;

      var transaction = BeginSaveTransaction();
      bool exception = false;

      try
      {
        JsonObject.Merge( data );
        Initialize();
      }
      catch
      {
        exception = true;
        throw;
      }
      finally
      {
        if ( exception == false )
          transaction.Dispose();
      }
    }


    /// <summary>
    /// 从 JSON 数据中初始化对象
    /// </summary>
    /// <param name="dataService">游戏数据服务</param>
    /// <param name="host">数据宿主</param>
    /// <param name="data">JSON 数据保存对象</param>
    internal void InitializeData( IGameDataService dataService, GameDataItem host, JsonDataObject data )
    {

      DataService = dataService;
      Host = host;

      _data = data;
      Initialize();

    }



    /// <summary>
    /// 用于同步的对象
    /// </summary>
    public object SyncRoot { get; private set; }




    public bool Initailzied { get; private set; }


    /// <summary>
    /// 初始化对象
    /// </summary>
    protected virtual void Initialize()
    {
      DataObject.Type = GetType().FullName;

      Initailzied = true;
    }


    protected void EnsureInitialized()
    {
      if ( Initailzied == false )
        throw new ObjectDisposedException( "对象尚未初始化或者已被销毁" );
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
    /// 数据宿主对象，如果有的话，则 Save 方法将重定向到该对象的 Save 方法
    /// </summary>
    protected GameDataItem Host { get; private set; }



    /// <summary>
    /// 通知数据服务保存数据对象
    /// </summary>
    protected virtual void Save()
    {
      if ( Host != null )
        Host.Save();

      lock ( _saveSync )
      {
        if ( _saveTransaction != null )
          return;


        EnsureInitialized();
        DataService.Save( this );
      }
    }



    private object _saveSync = new object();
    private IDisposable _saveTransaction;



    /// <summary>
    /// 开始一个保存事务，在该对象销毁时，所有修改才会被写入
    /// </summary>
    /// <returns>保存事务</returns>
    protected IDisposable BeginSaveTransaction()
    {
      if ( Host != null )
        return Host.BeginSaveTransaction();

      return new SaveTransaction( this );
    }

    private sealed class SaveTransaction : IDisposable
    {

      private GameDataItem _dataItem;

      public SaveTransaction( GameDataItem dataItem )
      {
        Monitor.Enter( dataItem._saveSync );
        dataItem._saveTransaction = this;
        _dataItem = dataItem;
      }


      void IDisposable.Dispose()
      {

        _dataItem._saveTransaction = null;
        _dataItem.Save();
        Monitor.Exit( _dataItem._saveSync );

      }
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
