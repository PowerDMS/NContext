// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonNetExtensions.cs">
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
//   Defines extension methods to support Json.NET serialization and deserialization.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace NContext.Extensions.JsonNet
{
    /// <summary>
    /// Defines extension methods to support Json.NET serialization and deserialization.
    /// </summary>
    public static class JsonNetExtensions
    {
        /// <summary>
        /// Deserializes the JSON-serialized stream and casts it to the object type specified.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>The deserialized object as type <typeparamref name="T"/></returns>
        public static T ReadAsJson<T>(this Stream stream) where T : class
        {
            return ReadAsJson(stream, typeof(T)) as T;
        }

        /// <summary>
        /// Deserializes the BSON-serialized stream and casts it to the object type specified.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>The deserialized object as type <typeparamref name="T"/></returns>
        public static T ReadAsBson<T>(this Stream stream) where T : class
        {
            return ReadAsBson(stream, typeof(T)) as T;
        }

        /// <summary>
        /// Reads the JSON-serialized stream and deserializes it into a CLR object.
        /// </summary>
        /// <param name="stream">The stream to deserialize.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns>The deserialized object.</returns>
        /// <remarks></remarks>
        public static Object ReadAsJson(this Stream stream, Type instanceType)
        {
            return ReadAsJson(stream, instanceType, null);
        }

        /// <summary>
        /// Reads the JSON-serialized stream and deserializes it into a CLR object.
        /// </summary>
        /// <param name="stream">The stream to deserialize.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="serializerSettings">The serializer settings.</param>
        /// <returns>The deserialized object.</returns>
        /// <remarks></remarks>
        public static Object ReadAsJson(this Stream stream, Type instanceType, JsonSerializerSettings serializerSettings)
        {
            if (stream == null)
            {
                return null;
            }

            using (var jsonTextReader = new JsonTextReader(new StreamReader(stream)))
            {
                return Deserialize(jsonTextReader, instanceType, serializerSettings);
            }
        }

        /// <summary>
        /// Reads the BSON-serialized stream and deserializes it into a CLR object.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns>The deserialized object.</returns>
        public static Object ReadAsBson(this Stream stream, Type instanceType)
        {
            return ReadAsBson(stream, instanceType, null);
        }

        /// <summary>
        /// Reads the BSON-serialized stream and deserializes it into a CLR object.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="serializerSettings">The serializer settings.</param>
        /// <returns>The deserialized object.</returns>
        /// <remarks></remarks>
        public static Object ReadAsBson(this Stream stream, Type instanceType, JsonSerializerSettings serializerSettings)
        {
            if (stream == null)
            {
                return null;
            }

            using (var bsonReader = new BsonReader(stream))
            {
                bsonReader.DateTimeKindHandling = DateTimeKind.Utc;

                return Deserialize(bsonReader, instanceType, serializerSettings);
            }
        }
        
        /// <summary>
        /// Serializes the object into JSON and writes the data into the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="serializerSettings">The serializer settings.</param>
        /// <remarks></remarks>
        public static void WriteAsJson(this Stream stream, Object instance, JsonSerializerSettings serializerSettings = null)
        {
            if (instance == null)
            {
                return;
            }

            using (var jsonTextWriter = new JsonTextWriter(new StreamWriter(stream)) { CloseOutput = false })
            {
                Serialize(jsonTextWriter, instance, serializerSettings);
            }
        }
        
        /// <summary>
        /// Serializes the object instance into BSON and writes the data into the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="instance">The object instance to serialize.</param>
        /// <param name="serializerSettings">The serializer settings.</param>
        /// <remarks></remarks>
        public static void WriteAsBson(this Stream stream, Object instance, JsonSerializerSettings serializerSettings = null)
        {
            if (instance == null)
            {
                return;
            }

            using (var bsonWriter = new BsonWriter(stream) { CloseOutput = false, DateTimeKindHandling = DateTimeKind.Utc })
            {
                Serialize(bsonWriter, instance, serializerSettings);
            }
        }

        private static Object Deserialize(JsonReader jsonReader, Type instanceType, JsonSerializerSettings serializerSettings)
        {
            try
            {
                using (jsonReader)
                {
                    return GetJsonSerializer(serializerSettings).Deserialize(jsonReader, instanceType);
                }
            }
            catch (JsonReaderException)
            {
                // TODO: (DG) Internal logging?...
                jsonReader.Close();
                throw;
            }
            catch (JsonSerializationException)
            {
                // TODO: (DG) Internal logging?...
                jsonReader.Close();
                throw;
            }
        }

        private static void Serialize(JsonWriter jsonWriter, Object instance, JsonSerializerSettings serializerSettings)
        {
            try
            {
                var jsonSerializer = GetJsonSerializer(serializerSettings);
                jsonSerializer.Serialize(jsonWriter, instance);
                jsonWriter.Flush();
            }
            catch (JsonWriterException)
            {
                // TODO: (DG) Internal logging?...
                jsonWriter.Close();
                throw;
            }
            catch (JsonSerializationException)
            {
                // TODO: (DG) Internal logging?...
                jsonWriter.Close();
                throw;
            }
        }

        private static JsonSerializer GetJsonSerializer(JsonSerializerSettings serializerSettings)
        {
            var jsonSerializerSettings = serializerSettings ??
                                         new JsonSerializerSettings
                                             {
                                                 MissingMemberHandling = MissingMemberHandling.Ignore,
                                                 NullValueHandling = NullValueHandling.Ignore,
                                                 ObjectCreationHandling = ObjectCreationHandling.Replace,
                                                 PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                                 TypeNameHandling = TypeNameHandling.Auto
                                             };

            return JsonSerializer.Create(jsonSerializerSettings);
        }
    }
}