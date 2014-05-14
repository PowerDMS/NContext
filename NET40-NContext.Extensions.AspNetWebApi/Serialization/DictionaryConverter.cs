namespace NContext.Extensions.AspNetWebApi.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    public class DictionaryConverter : JsonConverter
    {
        public override Boolean CanConvert(Type objectType)
        {
            return IsDictionary(objectType);
        }

        public override Boolean CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            return ReadValue(reader);
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            // CanWrite returns false
        }

        protected virtual IDictionary<String, Object> CreateDictionary()
        {
            return new Dictionary<String, Object>();
        }

        private static Boolean IsPrimitiveToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Undefined:
                case JsonToken.Null:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return true;
                default:
                    return false;
            }
        }

        private Object ReadValue(JsonReader reader)
        {
            while (reader.TokenType == JsonToken.Comment)
            {
                if (!reader.Read())
                {
                    throw new Exception("Unexpected end when reading the value.");
                }
            }

            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return ReadObject(reader);
                case JsonToken.StartArray:
                    return ReadList(reader);
                default:
                    if (IsPrimitiveToken(reader.TokenType))
                    {
                        return reader.Value;
                    }

                    throw new Exception("Unexpected token when reading the value.");
            }
        }

        private Object ReadList(JsonReader reader)
        {
            IList list = new List<Object>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;
                    case JsonToken.EndArray:
                        return list;
                    default:
                    {
                        var value = ReadValue(reader);
                        list.Add(value);
                        break;
                    }
                }
            }

            throw new Exception("Unexpected end when reading an array.");
        }

        private Object ReadObject(JsonReader reader)
        {
            var dictionary = CreateDictionary();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                    {
                        var propertyName = reader.Value.ToString();
                        if (!reader.Read())
                        {
                            throw new Exception("Unexpected end when reading the object.");
                        }

                        var value = ReadValue(reader);
                        dictionary[propertyName] = value;
                        break;
                    }
                    case JsonToken.Comment:
                        break;
                    case JsonToken.EndObject:
                        return dictionary;
                }
            }

            throw new Exception("Unexpected end when reading the object.");
        }

        private Boolean IsDictionary(Type type)
        {
            if (type == null) return false;

            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
                   type.GetInterfaces()
                       .Any(interfaceType => (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
                                             (interfaceType.Implements<IDictionary>()));
        }
    }
}