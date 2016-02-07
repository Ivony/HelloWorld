using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{


  /// <summary>
  /// 表示一个 JObject 属性改变的事件参数
  /// </summary>
  internal sealed class JsonPropertyChanged
  {

    public JsonPropertyChanged( JObject host, string propertyName, JToken originalValue, JToken newValue )
    {
      Host = host;
      PropertyName = propertyName;
      OriginalValue = originalValue;
      NewValue = newValue;
    }


    public JObject Host { get; private set; }

    public string PropertyName { get; private set; }

    public JToken OriginalValue { get; private set; }

    public JToken NewValue { get; private set; }

  }
}
