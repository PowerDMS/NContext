// Type: System.Net.Http.HttpRequestMessage
// Assembly: Microsoft.Net.Http, Version=0.3.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Projects\NContext\NContext.Services\bin\Release\Microsoft.Net.Http.dll

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace System.Net.Http
{
  public class HttpRequestMessage : IDisposable
  {
    private const int messageAlreadySent = 1;
    private const int messageNotYetSent = 0;
    private int sendStatus;
    private HttpMethod method;
    private Uri requestUri;
    private HttpRequestHeaders headers;
    private Version version;
    private HttpContent content;
    private IDictionary<string, object> properties;
    private bool disposed;

    public Version Version
    {
      get
      {
        return this.version;
      }
      set
      {
        if (value == (Version) null)
          throw new ArgumentNullException("value");
        this.CheckDisposed();
        this.version = value;
      }
    }

    public HttpContent Content
    {
      get
      {
        return this.content;
      }
      set
      {
        this.CheckDisposed();
        this.content = value;
      }
    }

    public HttpMethod Method
    {
      get
      {
        return this.method;
      }
      set
      {
        if (value == (HttpMethod) null)
          throw new ArgumentNullException("value");
        this.CheckDisposed();
        this.method = value;
      }
    }

    public IDictionary<string, object> Properties
    {
      get
      {
        if (this.properties == null)
          this.properties = (IDictionary<string, object>) new Dictionary<string, object>();
        return this.properties;
      }
    }

    public Uri RequestUri
    {
      get
      {
        return this.requestUri;
      }
      set
      {
        if (value != (Uri) null && value.IsAbsoluteUri && !HttpUtilities.IsHttpUri(value))
          throw new ArgumentException("Only 'http' and 'https' schemes are allowed.", "value");
        this.CheckDisposed();
        this.requestUri = value;
      }
    }

    public HttpRequestHeaders Headers
    {
      get
      {
        if (this.headers == null)
          this.headers = new HttpRequestHeaders();
        return this.headers;
      }
    }

    public HttpRequestMessage()
      : this(HttpMethod.Get, (Uri) null)
    {
    }

    public HttpRequestMessage(HttpMethod method, Uri requestUri)
    {
      this.InitializeValues(method, requestUri);
    }

    public HttpRequestMessage(HttpMethod method, string requestUri)
    {
      if (string.IsNullOrEmpty(requestUri))
        this.InitializeValues(method, (Uri) null);
      else
        this.InitializeValues(method, new Uri(requestUri, UriKind.RelativeOrAbsolute));
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Method: ");
      stringBuilder.Append((object) this.method);
      stringBuilder.Append(", RequestUri: '");
      stringBuilder.Append(this.requestUri == (Uri) null ? "<null>" : this.requestUri.ToString());
      stringBuilder.Append("', Version: ");
      stringBuilder.Append((object) this.version);
      stringBuilder.Append(", Content: ");
      stringBuilder.Append(this.content == null ? "<null>" : this.content.GetType().FullName);
      stringBuilder.Append(", Headers:\r\n");
      stringBuilder.Append(HeaderUtilities.DumpHeaders((HttpHeaders) this.headers, this.content == null ? (HttpHeaders) null : (HttpHeaders) this.content.Headers));
      return ((object) stringBuilder).ToString();
    }

    private void InitializeValues(HttpMethod method, Uri requestUri)
    {
      if (method == (HttpMethod) null)
        throw new ArgumentNullException("method");
      if (requestUri != (Uri) null && requestUri.IsAbsoluteUri && !HttpUtilities.IsHttpUri(requestUri))
        throw new ArgumentException("Only 'http' and 'https' schemes are allowed.", "requestUri");
      this.method = method;
      this.requestUri = requestUri;
      this.version = HttpUtilities.DefaultVersion;
    }

    internal bool MarkAsSent()
    {
      return Interlocked.Exchange(ref this.sendStatus, 1) == 0;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.disposed)
        return;
      this.disposed = true;
      if (this.content == null)
        return;
      this.content.Dispose();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(this.GetType().FullName);
    }
  }
}
