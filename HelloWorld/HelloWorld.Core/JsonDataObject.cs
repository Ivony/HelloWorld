using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloWorld
{
  internal sealed class JsonDataObject : JObject, IDataHost
  {


    private IDataHost _host;

    internal JsonDataObject( IDataHost host )
    {
      _host = host;
    }



    public JsonDataObject( JObject data, IDataHost host ) : this( host )
    {
      foreach ( var property in data.Properties().Select( ConvertProperty ) )
        Add( property );
    }


    private JProperty ConvertProperty( JProperty property )
    {
      var value = ConvertValue( property.Value, _host );

      if ( value == property.Value )
        return property;

      return new JProperty( property.Name, value );
    }


    internal static JToken ConvertValue( JToken data, IDataHost host )
    {
      if ( data == null )
        return null;


      var dataHost = data as IDataHost;
      if ( dataHost != null && dataHost.Token == host.Token )
        return data;


      var obj = data as JObject;

      if ( obj != null )
        return new JsonDataObject( obj, host );


      var array = data as JArray;
      if ( array != null )
        return new JsonDataArray( array, host );

      if ( data is JContainer )
        throw new InvalidOperationException();

      return data;

    }



    protected override void OnPropertyChanged( string propertyName )
    {

      var value = ConvertValue( this[propertyName], _host );
      if ( value == this[propertyName] )
        return;

      this[propertyName] = value;

      _host.Save( this );
    }


    public void Save( JToken data )
    {
      _host.Save( this );
    }

    Guid IDataHost.Token { get { return _host.Token; } }

  }
}
