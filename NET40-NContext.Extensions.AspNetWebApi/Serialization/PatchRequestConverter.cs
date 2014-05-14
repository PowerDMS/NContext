// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatchRequest.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.AspNetWebApi.Serialization
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;

    using NContext.Extensions.AspNetWebApi.Patching;

    using Newtonsoft.Json;

    /// <summary>
    /// Enforces dictionary deserialization for JSON.NET to use proper C# containers 
    /// instead of JToken, JObject, JProperty, and JValue internal types.
    /// </summary>
    public class PatchRequestConverter : DictionaryConverter
    {
        private static readonly ConcurrentDictionary<Type, ConstructorInfo> _TypeConstructorCache;

        private readonly Func<Type, ConstructorInfo> _PatchRequestConstructor;

        private Boolean _PatchRequestCreated;

        private Type _PatchRequestType;

        static PatchRequestConverter()
        {
            _TypeConstructorCache = new ConcurrentDictionary<Type, ConstructorInfo>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchRequestConverter"/> class.
        /// </summary>
        public PatchRequestConverter()
        {
            _PatchRequestConstructor = type =>
            {
                var patchRequest = typeof(PatchRequest<>).MakeGenericType(type);
                var ctor = patchRequest.GetConstructor(Type.EmptyTypes);
                if (ctor == null)
                {
                    throw new InvalidOperationException(String.Format("The type: {0} must contain a default constructor.", type.FullName));
                }

                return ctor;
            };
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof (PatchRequest<>);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            if (!_PatchRequestCreated &&
                objectType.IsGenericType &&
                objectType.GetGenericTypeDefinition() == typeof(PatchRequest<>))
            {
                _PatchRequestType = objectType.GetGenericArguments()[0];
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Creates the dictionary.
        /// </summary>
        /// <returns>IDictionary&lt;String, Object&gt;.</returns>
        protected override IDictionary<String, Object> CreateDictionary()
        {
            if (_PatchRequestCreated)
            {
                return base.CreateDictionary();
            }

            _PatchRequestCreated = true;

            return
                (IDictionary<String, Object>)
                    _TypeConstructorCache.GetOrAdd(_PatchRequestType, _PatchRequestConstructor).Invoke(null);
        }
    }
}