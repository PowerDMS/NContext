// Type: Microsoft.ApplicationServer.Http.MediaTypeFormatterCollectionExtensions
// Assembly: Microsoft.ApplicationServer.HttpEnhancements, Version=0.3.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Projects\NContext\packages\WebApi.Enhancements.0.5.0\lib\40-Full\Microsoft.ApplicationServer.HttpEnhancements.dll

using System.Collections.ObjectModel;
using System.Net.Http.Formatting;

namespace Microsoft.ApplicationServer.Http
{
  public static class MediaTypeFormatterCollectionExtensions
  {
    public static void AddRange(this MediaTypeFormatterCollection formatters, params MediaTypeFormatter[] formattersToAdd)
    {
      foreach (MediaTypeFormatter mediaTypeFormatter in formattersToAdd)
        ((Collection<MediaTypeFormatter>) formatters).Add(mediaTypeFormatter);
    }
  }
}
