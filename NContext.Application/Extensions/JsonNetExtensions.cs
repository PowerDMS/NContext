// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonNetExtensions.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
// <summary>
//   Defines extension methods to support Json.NET serialization and deserialization.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace NContext.Application.Extensions
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

        /// <summary>
        /// Gets the json serializer which supports references and circular referencing.
        /// </summary>
        /// <returns>The <see cref="JsonSerializer"/>.</returns>
        /// <remarks></remarks>
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
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Objects
                };
        }

        /// <summary>
        /// Deserializes the specified <see cref="JsonReader"/> stream.
        /// </summary>
        /// <param name="jsonReader">The json reader.</param>
        /// <param name="instanceType">Type of the instance to deserialize.</param>
        /// <returns>The deserialized object.</returns>
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

        /// <summary>
        /// Serializes the specified <see cref="JsonWriter"/> stream.
        /// </summary>
        /// <param name="jsonWriter">The json writer.</param>
        /// <param name="instance">The instance to serialize.</param>
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