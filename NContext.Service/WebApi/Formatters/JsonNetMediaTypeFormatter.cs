// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonNetMediaTypeFormatter.cs">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
//
// <summary>
//   Defines a JSON MediaTypeProcessor using Json.NET serialization/deserialization.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

using NContext.Extensions.JsonNet;

namespace NContext.Service.WebApi.Formatters
{
    /// <summary>
    /// Defines a Json <see cref="MediaTypeFormatter"/> using Json.NET serialization/deserialization.
    /// </summary>
    public class JsonNetMediaTypeFormatter : JsonMediaTypeFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetMediaTypeFormatter"/> class.
        /// </summary>
        public JsonNetMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json") { CharSet = "utf-8" });
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json") { CharSet = "utf-8" });
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/bson") { CharSet = "utf-8" });
        }

        /// <summary>
        /// Called when [read from stream].
        /// </summary>
        /// <param name="type">The type of object to deserialize.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="httpContentHeaders">The HTTP content headers.</param>
        /// <returns>The de-serialized object.</returns>
        /// <remarks></remarks>
        protected override Object OnReadFromStream(Type type, Stream stream, HttpContentHeaders httpContentHeaders)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            return httpContentHeaders.ContentType.MediaType.ToLower() == "application/bson"
                ? stream.ReadAsBsonSerializable(type)
                : stream.ReadAsJsonSerializable(type);
        }

        /// <summary>
        /// Called to write an object to the <paramref name="stream"/>.
        /// </summary>
        /// <param name="type">The type of object to write.</param>
        /// <param name="value">The object instance to write.</param>
        /// <param name="stream">The <see cref="T:System.IO.Stream"/> to which to write.</param>
        /// <param name="httpContentHeaders">The HTTP content headers.</param>
        /// <param name="context">The <see cref="T:System.Net.TransportContext"/>.</param>
        /// <remarks></remarks>
        protected override void OnWriteToStream(Type type, Object value, Stream stream, HttpContentHeaders httpContentHeaders, TransportContext context)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (httpContentHeaders.ContentType.MediaType.ToLower() == "application/bson")
            {
                stream.WriteAsBsonSerializable(value);
            }
            else
            {
                stream.WriteAsJsonSerializable(value);
            }
        }
    }
}