using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Linq;

namespace HelloWorld.WebHost
{
  internal class JsonContentNegotiator : IContentNegotiator
  {
    public ContentNegotiationResult Negotiate( Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters )
    {
      var formatter = formatters.OfType<JsonMediaTypeFormatter>().First();
      var mediaType = formatter.SupportedMediaTypes.First();
      return new ContentNegotiationResult( formatter, mediaType );

    }
  }
}