// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlDataContractMediaTypeFormatter.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines an Xml implementation of IContentFormatter using DataContractSerializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization;

namespace NContext.Application.Services.Formatters
{
    /// <summary>
    /// Defines an Xml implementation of <see cref="MediaTypeFormatter"/> using <see cref="DataContractSerializer"/>.
    /// </summary>
    public sealed class XmlDataContractMediaTypeFormatter : MediaTypeFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataContractMediaTypeFormatter"/> class.
        /// </summary>
        public XmlDataContractMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
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
            var serializer = new DataContractSerializer(type, null, Int32.MaxValue, false, true, null);
            return serializer.ReadObject(stream);
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
            var serializer = new DataContractSerializer(type, null, Int32.MaxValue, false, true, null);
            serializer.WriteObject(stream, value);
        }
    }
}
