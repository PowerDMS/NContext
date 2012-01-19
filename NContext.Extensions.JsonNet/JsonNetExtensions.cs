// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonNetExtensions.cs">
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
        public static T ReadAsJsonSerializable<T>(this Stream stream) where T : class
        {
            return ReadAsJsonSerializable(stream, typeof(T)) as T;
        }

        /// <summary>
        /// Deserializes the BSON-serialized stream and casts it to the object type specified.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>The deserialized object as type <typeparamref name="T"/></returns>
        public static T ReadAsBsonSerializable<T>(this Stream stream) where T : class
        {
            return ReadAsBsonSerializable(stream, typeof(T)) as T;
        }

        /// <summary>
        /// Reads the JSON-serialized stream and deserializes it into a CLR object.
        /// </summary>
        /// <param name="stream">The stream to deserialize.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns>The deserialized object.</returns>
        /// <remarks></remarks>
        public static Object ReadAsJsonSerializable(this Stream stream, Type instanceType)
        {
            if (stream == null)
            {
                return null;
            }

            using (var jsonTextReader = new JsonTextReader(new StreamReader(stream)))
            {
                return Deserialize(jsonTextReader, instanceType);
            }
        }

        /// <summary>
        /// Reads the BSON-serialized stream and deserializes it into a CLR object.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns>The deserialized object.</returns>
        public static Object ReadAsBsonSerializable(this Stream stream, Type instanceType)
        {
            if (stream == null)
            {
                return null;
            }

            using (var bsonReader = new BsonReader(stream))
            {
                bsonReader.DateTimeKindHandling = DateTimeKind.Utc;

                return Deserialize(bsonReader, instanceType);
            }
        }

        /// <summary>
        /// Serializes the object into JSON and writes the data into the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="instance">The object instance to serialize.</param>
        public static void WriteAsJsonSerializable(this Stream stream, Object instance)
        {
            if (instance == null)
            {
                return;
            }

            using (var jsonTextWriter = new JsonTextWriter(new StreamWriter(stream)) { CloseOutput = false })
            {
                Serialize(jsonTextWriter, instance);
            }
        }

        /// <summary>
        /// Serializes the object instance into BSON and writes the data into the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="instance">The object instance to serialize.</param>
        public static void WriteAsBsonSerializable(this Stream stream, Object instance)
        {
            if (instance == null)
            {
                return;
            }

            using (var bsonWriter = new BsonWriter(stream) { CloseOutput = false, DateTimeKindHandling = DateTimeKind.Utc })
            {
                Serialize(bsonWriter, instance);
            }
        }

        private static JsonSerializer GetJsonSerializer()
        {
            return new JsonSerializer
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
        }

        private static JsonSerializer GetBsonSerializer()
        {
            return new JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        private static Object Deserialize(JsonReader jsonReader, Type instanceType)
        {
            try
            {
                using (jsonReader)
                {
                    var jsonSerializer = jsonReader is BsonReader 
                        ? GetBsonSerializer()
                        : GetJsonSerializer();
                    
                    return jsonSerializer.Deserialize(jsonReader, instanceType);
                }
            }
            catch (JsonReaderException)
            {
                // TODO: (DG) Internal logging...
                jsonReader.Close();
                throw;
            }
            catch (JsonSerializationException)
            {
                // TODO: (DG) Internal logging...
                jsonReader.Close();
                throw;
            }
        }

        private static void Serialize(JsonWriter jsonWriter, Object instance)
        {
            try
            {
                var jsonSerializer = jsonWriter is BsonWriter
                        ? GetBsonSerializer()
                        : GetJsonSerializer();

                jsonSerializer.Serialize(jsonWriter, instance);
                jsonWriter.Flush();
            }
            catch (JsonWriterException)
            {
                // TODO: (DG) Internal logging...
                jsonWriter.Close();
                throw;
            }
            catch (JsonSerializationException)
            {
                // TODO: (DG) Internal logging...
                jsonWriter.Close();
                throw;
            }
        }
    }
}