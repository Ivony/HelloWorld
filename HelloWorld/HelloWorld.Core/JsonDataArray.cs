using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.ComponentModel;

namespace HelloWorld
{
  internal class JsonDataArray : JArray, IDataHost
  {


    private IDataHost _host;

    public JsonDataArray( JArray array, IDataHost host )
    {

      foreach ( var item in array.Select( item => JsonDataObject.ConvertValue( item, host ) ) )
        Add( item );

      _host = host;

    }


    protected override void OnListChanged( ListChangedEventArgs e )
    {
      base.OnListChanged( e );

      Save( this );
    }

    public void Save( JToken data )
    {
      _host.Save( this );
    }
  }
}