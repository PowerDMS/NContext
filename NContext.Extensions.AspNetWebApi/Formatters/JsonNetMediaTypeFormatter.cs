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
using System.Text;
using System.Threading.Tasks;

using NContext.Extensions.JsonNet;

namespace NContext.Extensions.AspNetWebApi.Formatters
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
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/bson"));
            Encoding = new UTF8Encoding(false, true);
        }

        /// <summary>
        /// Determines whether this <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter"/> can read objects of the specified type.
        /// </summary>
        /// <param name="type">The type of object that will be read.</param>
        /// <returns>true if objects of this type can be read, otherwise false.</returns>
        /// <remarks></remarks>
        protected override Boolean CanReadType(Type type)
        {
            return CanReadTypeInternal(type);
        }

        /// <summary>
        /// Determines whether this <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter"/> can write objects of the specified type.
        /// </summary>
        /// <param name="type">The type of object that will be written.</param>
        /// <returns>true if objects of this type can be written, otherwise false.</returns>
        /// <remarks></remarks>
        protected override Boolean CanWriteType(Type type)
        {
            return CanReadTypeInternal(type);
        }

        /// <summary>
        /// Called when [read from stream async].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentHeaders">The content headers.</param>
        /// <param name="formatterContext">The formatter context.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected override Task<Object> OnReadFromStreamAsync(Type type, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            return Task.Factory.StartNew(
                () =>
                    {
                        return contentHeaders.ContentType.MediaType.Equals(
                            "application/bson", StringComparison.InvariantCultureIgnoreCase)
                                   ? stream.ReadAsBson(type)
                                   : stream.ReadAsJson(type);
                    });
        }

        /// <summary>
        /// Called during serialization to write an object of the specified type to the specified stream.
        /// </summary>
        /// <param name="type">The type of object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="stream">The <see cref="T:System.IO.Stream"/> to which to write.</param>
        /// <param name="contentHeaders">The <see cref="T:System.Net.Http.Headers.HttpContentHeaders"/> for the content being written.</param>
        /// <param name="formatterContext">The <see cref="T:System.Net.Http.Formatting.FormatterContext"/> containing the respective request or response.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext"/>.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> that will write the value to the stream.</returns>
        /// <remarks></remarks>
        protected override Task OnWriteToStreamAsync(Type type, Object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            return Task.Factory.StartNew(
                () =>
                    {
                        if (contentHeaders.ContentType.MediaType.Equals("application/bson", StringComparison.InvariantCultureIgnoreCase))
                        {
                            stream.WriteAsBson(value);
                        }
                        else
                        {
                            stream.WriteAsJson(value);
                        }
                    });
        }

        private static Boolean CanReadTypeInternal(Type type)
        {
            return type != typeof(IKeyValueModel);
        }
    }
}