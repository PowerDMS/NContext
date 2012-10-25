// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonNetMediaTypeFormatter.cs">
//   Copyright (c) 2012
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
using System.Text;
using System.Threading.Tasks;

namespace NContext.Extensions.AspNetWebApi.Formatters
{
    using Newtonsoft.Json;

    /// <summary>
    /// Defines a Json.NET <see cref="MediaTypeFormatter"/> with support for BSON.
    /// </summary>
    public class JsonNetMediaTypeFormatter : MediaTypeFormatter
    {
        private static Lazy<JsonSerializerSettings> _JsonSerializerSettings = 
            new Lazy<JsonSerializerSettings>(() => null);

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetMediaTypeFormatter" /> class.
        /// </summary>
        public JsonNetMediaTypeFormatter() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="serializerSettingsFactory">The serializer settings factory.</param>
        public JsonNetMediaTypeFormatter(Func<JsonSerializerSettings> serializerSettingsFactory)
        {
            SupportedMediaTypes.Add(MediaTypeConstants.ApplicationJsonMediaType);
            SupportedMediaTypes.Add(MediaTypeConstants.TextJsonMediaType);
            SupportedMediaTypes.Add(MediaTypeConstants.ApplicationBsonMediaType);

            if (serializerSettingsFactory != null && !_JsonSerializerSettings.IsValueCreated)
            {
                _JsonSerializerSettings = new Lazy<JsonSerializerSettings>(serializerSettingsFactory);
            }
        }

        /// <summary>
        /// Determines whether this <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> can read objects of the specified <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type of object that will be read.</param>
        /// <returns>true if objects of this <paramref name="type" /> can be read, otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public override Boolean CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return true;
        }

        /// <summary>
        /// Determines whether this <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> can write objects of the specified <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type of object that will be written.</param>
        /// <returns>true if objects of this <paramref name="type" /> can be written, otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public override Boolean CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return true;
        }

        /// <summary>
        /// Reads an object of the specified <paramref name="type" /> from the specified <paramref name="readStream" />. This method is called during deserialization.
        /// </summary>
        /// <param name="type">The type of object to read.</param>
        /// <param name="readStream">The stream from which to read</param>
        /// <param name="content">The content being written.</param>
        /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
        /// <returns>Returns <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
        public override Task<Object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (readStream == null)
            {
                throw new ArgumentNullException("readStream");
            }

            var taskCompletionSource = new TaskCompletionSource<Object>();
            try
            {
                taskCompletionSource.SetResult(
                    content.Headers.ContentType.Equals(MediaTypeConstants.ApplicationBsonMediaType)
                        ? readStream.ReadAsBson(type, _JsonSerializerSettings.Value)
                        : readStream.ReadAsJson(type, _JsonSerializerSettings.Value));
            }
            catch (Exception exception)
            {
                taskCompletionSource.SetException(exception);
            }

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Writes an object of the specified <paramref name="type" /> to the specified <paramref name="writeStream" />. This method is called during serialization.
        /// </summary>
        /// <param name="type">The type of object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> where the content is being written.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" />.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that will write the value to the stream.</returns>
        public override Task WriteToStreamAsync(Type type, Object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (writeStream == null)
            {
                throw new ArgumentNullException("writeStream");
            }

            var taskCompletionSource = new TaskCompletionSource<Object>();
            try
            {
                if (content.Headers.ContentType.Equals(MediaTypeConstants.ApplicationBsonMediaType))
                {
                    writeStream.WriteAsBson(value, _JsonSerializerSettings.Value);
                }
                else
                {
                    writeStream.WriteAsJson(value, _JsonSerializerSettings.Value);
                }

                taskCompletionSource.SetResult(null);
            }
            catch (Exception exception)
            {
                taskCompletionSource.SetException(exception);
            }

            return taskCompletionSource.Task;
        }
    }
}