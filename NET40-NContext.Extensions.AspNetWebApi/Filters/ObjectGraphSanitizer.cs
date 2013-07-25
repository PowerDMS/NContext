// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectGraphSanitizer.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AspNetWebApi.Filters
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    using NContext.Text;

    /// <summary>
    /// Defines a object graph traverser designed for sanitizing user-input.
    /// </summary>
    public class ObjectGraphSanitizer
    {
        private readonly ITextSanitizer _TextSanitizer;

        private readonly Int32 _MaxDegreeOfParallelism;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectGraphSanitizer"/> class.
        /// </summary>
        /// <param name="textSanitizer">The text sanitizer.</param>
        /// <param name="maxDegreeOfParallelism">The degree of parallelism to invoke sanitization.</param>
        public ObjectGraphSanitizer(ITextSanitizer textSanitizer, Int32 maxDegreeOfParallelism)
        {
            _TextSanitizer = textSanitizer;
            _MaxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// Traverses the specified object graph looking for all strings to be sanitized by <see cref="ITextSanitizer"/>.
        /// </summary>
        /// <param name="objectToSanitize">The object to sanitize.</param>
        public void Sanitize(Object objectToSanitize)
        {
            if (objectToSanitize == null || IsTerminalObject(objectToSanitize.GetType()))
            {
                return;
            }

            var idGenerator = new ObjectIDGenerator();
            var stack = new Stack<Node>();
            var sanitizableNodes = new ConcurrentBag<Node>();
            var nodeEqualityComparer = new NodeEqualityComparer();

            stack.Push(new Node(null, objectToSanitize, null));

            while (stack.Count > 0)
            {
                Node currentItem = stack.Pop();

                if (currentItem.Value == null) continue;

                Boolean firstOccurrence;
                idGenerator.GetId(currentItem.Value, out firstOccurrence);

                /* There is no way to assign a unique ID to a string as far as I can tell.
                 * Therefore, any strings encountered with the same value will be sanitized
                 * each time.
                 * */
                if (!firstOccurrence && !(currentItem.Value is String))
                {
                    continue; // Should never get here because of WHERE filter below.
                }

                var currentItemType = currentItem.PropertyInfo == null ? currentItem.Value.GetType() : currentItem.PropertyInfo.PropertyType;
                if (!IsTerminalObject(currentItemType))
                {
                    if (!(currentItemType.IsGenericType))
                    {
                        Boolean propertyValueHasId;

#if NET40
                        (from property in currentItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         let propertyValue = property.GetValue(currentItem.Value, null)
                         where (property.PropertyType == typeof(String) && 
                                !String.IsNullOrWhiteSpace(propertyValue as String)) ||
                               (propertyValue != null && 
                                !IsTerminalObject(propertyValue.GetType()) && 
                                idGenerator.HasId(propertyValue, out propertyValueHasId) == 0)
                         select new Node(currentItem.Value, propertyValue, property))
                         .ForEach(stack.Push);
#elif NET45_OR_GREATER
                        
                        (from property in currentItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         let propertyValue = property.GetValue(currentItem.Value)
                         where (property.PropertyType == typeof(String) && 
                                !String.IsNullOrWhiteSpace(propertyValue as String)) ||
                               (propertyValue != null && 
                                !IsTerminalObject(propertyValue.GetType()) && 
                                idGenerator.HasId(propertyValue, out propertyValueHasId) == 0)
                         select new Node(currentItem.Value, propertyValue, property))
                         .ForEach(stack.Push);
#endif
                    }
                    else if (IsDictionary(currentItemType))
                    {
                        var valueType = currentItemType.GetGenericArguments()[1];
                        if ((valueType == typeof(String) || valueType == typeof(Object)) && !sanitizableNodes.Contains(currentItem, nodeEqualityComparer))
                        {
                            sanitizableNodes.Add(currentItem);
                        }
                        else if (!IsTerminalObject(valueType))
                        {
                            var dictionary = ((IDictionary)currentItem.Value);
                            var enumerator = dictionary.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                stack.Push(new Node(currentItem.Parent, enumerator.Value, currentItem.PropertyInfo));
                            }
                        }
                    }
                    else if (IsEnumerable(currentItemType))
                    {
                        var genericArgument = currentItemType.GetGenericArguments()[0];
                        if ((genericArgument == typeof(String) || genericArgument == typeof(Object)) && !sanitizableNodes.Contains(currentItem, nodeEqualityComparer))
                        {
                            sanitizableNodes.Add(currentItem);
                        }
                        else if (!IsTerminalObject(genericArgument))
                        {
                            ((IEnumerable)currentItem.Value)
                                .Cast<Object>()
                                .Select(item => new Node(currentItem.Parent, item, currentItem.PropertyInfo))
                                .ForEach(stack.Push);
                        }
                    }
                }
                else if (currentItem.Parent != null && currentItem.PropertyInfo != null && !sanitizableNodes.Contains(currentItem, nodeEqualityComparer))
                {
                    sanitizableNodes.Add(currentItem);
                }
            }

            sanitizableNodes.AsParallel()
                            .WithDegreeOfParallelism(_MaxDegreeOfParallelism)
                            .ForAll(node =>
                                {
                                    var nodeValueType = node.PropertyInfo == null
                                                            ? node.Value.GetType()
                                                            : node.PropertyInfo.PropertyType;

                                    if (IsTerminalObject(nodeValueType) && node.Parent != null && node.PropertyInfo != null)
                                    {
#if NET40
                                        node.PropertyInfo.SetValue(node.Parent, _TextSanitizer.Sanitize((String)node.Value), null);
#elif NET45_OR_GREATER
                                        node.PropertyInfo.SetValue(node.Parent, _TextSanitizer.Sanitize((String)node.Value));
#endif
                                    }
                                    else if (IsDictionary(nodeValueType))
                                    {
                                        var dictionary = ((IDictionary)node.Value);
                                        var keyList = new List<Object>();

                                        var enumerator = dictionary.GetEnumerator();
                                        while (enumerator.MoveNext())
                                        {
                                            var entry = (DictionaryEntry) enumerator.Current;
                                            if (entry.Value == null)
                                            {
                                                continue;
                                            }

                                            var entryValueType = entry.Value.GetType();
                                            if (entryValueType == typeof(String) && !String.IsNullOrWhiteSpace(entry.Value as String))
                                            {
                                                keyList.Add(entry.Key);
                                            }
                                        }

                                        foreach (var key in keyList)
                                        {
                                            dictionary[key] = _TextSanitizer.Sanitize((String)dictionary[key]);
                                        }
                                    }
                                    else
                                    {
                                        if (node.Parent == null && node.PropertyInfo == null && node.Value as IList<String> != null)
                                        {
                                            var enumerable = (IList<String>)node.Value;
                                            for (var j = 0; j < enumerable.Count; j++)
                                            {
                                                if (!String.IsNullOrWhiteSpace(enumerable[j]))
                                                {
                                                    enumerable[j] = _TextSanitizer.Sanitize((String)enumerable[j]);
                                                }
                                            }
                                        }
                                        else if (node.Parent == null && node.PropertyInfo == null && node.Value as IList<Object> != null)
                                        {
                                            var enumerable = (IList<Object>)node.Value;
                                            for (var j = 0; j < enumerable.Count; j++)
                                            {
                                                if (enumerable[j] is String && !String.IsNullOrWhiteSpace(enumerable[j] as String))
                                                {
                                                    enumerable[j] = _TextSanitizer.Sanitize((String)enumerable[j]);
                                                }
                                            }
                                        }
                                        else if (node.Parent != null && node.PropertyInfo != null)
                                        {
                                            var sanitizedStringCollection = new Collection<String>();
                                            ((IEnumerable<String>) node.Value)
                                                .ForEach(
                                                    value =>
                                                    sanitizedStringCollection.Add(String.IsNullOrWhiteSpace(value)
                                                                                      ? value
                                                                                      : _TextSanitizer.Sanitize(value)));

#if NET40
                                            node.PropertyInfo.SetValue(node.Parent, sanitizedStringCollection, null);
#elif NET45_OR_GREATER
                                            node.PropertyInfo.SetValue(node.Parent, sanitizedStringCollection);
#endif
                                        }
                                    }
                                });
        }

        private Boolean IsDictionary(Type type)
        {
            return type != null &&
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
                type.GetInterfaces()
                    .Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private Boolean IsEnumerable(Type type)
        {
            if (type == null) return false;

            return 
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) || 
                type.GetInterfaces()
                    .Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private Boolean IsTerminalObject(Type type)
        {
            if (type == null) return true;

            return type.IsPrimitive || 
                   type.IsEnum || 
                   type.IsPointer ||
                   type == typeof(String) ||
                   type == typeof(DateTime) ||
                   type == typeof(Decimal) ||
                   type == typeof(Guid) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan);
        }

        private class Node
        {
            private readonly Object _Parent;

            private readonly Object _Value;

            private readonly PropertyInfo _PropertyInfo;

            public Node(Object parent, Object value, PropertyInfo propertyInfo)
            {
                _Parent = parent;
                _PropertyInfo = propertyInfo;
                _Value = value;
            }

            public Object Parent
            {
                get
                {
                    return _Parent;
                }
            }

            public Object Value
            {
                get
                {
                    return _Value;
                }
            }

            public PropertyInfo PropertyInfo
            {
                get
                {
                    return _PropertyInfo;
                }
            }
        }

        private class NodeEqualityComparer : IEqualityComparer<Node>
        {
            public Boolean Equals(Node x, Node y)
            {
                return x.Parent == y.Parent && x.PropertyInfo == y.PropertyInfo;
            }

            public Int32 GetHashCode(Node node)
            {
                return new { A = node.Parent, B = node.PropertyInfo }.GetHashCode();
            }
        }
    }
}