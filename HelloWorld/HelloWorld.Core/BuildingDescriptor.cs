using Newtonsoft.Json.Linq;

namespace HelloWorld.Core
{

  /// <summary>
  /// 建筑
  /// </summary>
  public class BuildingDescriptor
  {


    private BuildingDescriptor() { }

    internal BuildingDescriptor Create( JObject data )
    {
      if ( data == null )
        return null;

      return new BuildingDescriptor
      {
        Name = data.Value<string>( "Name" ),
        Description = data.Value<string>( "Description" ),
        _json = data.ToString(),
      };
    }


    public string Name
    {
      get; private set;
    }

    public string Description
    {
      get; private set;
    }


    private string _json;

    public override string ToString() { return _json; }



  }
}