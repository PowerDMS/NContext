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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    using NContext.Text;

    /// <summary>
    /// Defines a object graph traverser designed for sanitizing user-input.
    /// </summary>
    public class ObjectGraphSanitizer
    {
        private readonly ISanitizeText _TextSanitizer;

        private readonly ParallelOptions _ParallelOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectGraphSanitizer"/> class.
        /// </summary>
        /// <param name="textSanitizer">The text sanitizer.</param>
        /// <param name="maxDegreeOfParallelism">The degree of parallelism to invoke sanitization.</param>
        public ObjectGraphSanitizer(ISanitizeText textSanitizer, Int32 maxDegreeOfParallelism)
        {
            _TextSanitizer = textSanitizer;
            _ParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
        }

        /// <summary>
        /// Traverses the specified object graph looking for all strings to be sanitized by <see cref="ISanitizeText"/>.
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
            var sanitizableNodes = new HashSet<Node>();

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
                    if (!(currentItemType.IsGenericType) && currentItemType.GetInterfaces().All(i => !i.IsGenericType))
                    {
                        Boolean propertyValueHasId;

                        /* Get all properties that are either:
                         * a) strings that aren't null or whitespace
                         * b) non-terminal objects that we haven't already visited in the graph.
                         * */
                        (from property in currentItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         let propertyValue = property.GetValue(currentItem.Value, null)
                         where (property.PropertyType == typeof(String) && 
                                !String.IsNullOrWhiteSpace(propertyValue as String)) ||
                               (propertyValue != null && 
                                !IsTerminalObject(propertyValue.GetType()) && 
                                idGenerator.HasId(propertyValue, out propertyValueHasId) == 0)
                         select new Node(currentItem.Value, propertyValue, property))
                         .ForEach(stack.Push);
                    }
                    else if (IsDictionary(currentItemType))
                    {
                        var genericDictionaryInterface =
                            currentItemType.IsGenericType &&
                            currentItemType.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                                ? currentItemType
                                : currentItemType.GetInterfaces()
                                                 .Single(
                                                     interfaceType =>
                                                     interfaceType.IsGenericType &&
                                                     interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>));

                        var genericDictionaryValueType = genericDictionaryInterface.GetGenericArguments().Last();
                        if (genericDictionaryValueType == typeof(String))
                        {
                            sanitizableNodes.Add(currentItem);
                        }
                        else if (genericDictionaryValueType == typeof(Object))
                        {
                            sanitizableNodes.Add(currentItem);
                            var dictionary = ((IDictionary)currentItem.Value);
                            var enumerator = dictionary.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                if (enumerator.Value != null && !IsTerminalObject(enumerator.Value.GetType()))
                                {
                                    stack.Push(new Node(null, enumerator.Value, null));
                                }
                            }
                        }
                        else if (!IsTerminalObject(genericDictionaryValueType))
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
                        var genericEnumerableInterface =
                            currentItemType.IsGenericType &&
                            currentItemType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                                ? currentItemType
                                : currentItemType.GetInterfaces()
                                                 .Single(
                                                     interfaceType =>
                                                     interfaceType.IsGenericType &&
                                                     interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                        var genericArgumentType = genericEnumerableInterface.GetGenericArguments().Single();
                        if (genericArgumentType == typeof(String))
                        {
                            sanitizableNodes.Add(currentItem);
                        }
                        else if (genericArgumentType == typeof(Object))
                        {
                            sanitizableNodes.Add(currentItem);
                            ((IEnumerable)currentItem.Value)
                                .Cast<Object>()
                                .Where(element => element != null && !IsTerminalObject(element.GetType()))
                                .Select(element => new Node(null, element, null))
                                .ForEach(stack.Push);
                        }
                        else if (!IsTerminalObject(genericArgumentType))
                        {
                            ((IEnumerable)currentItem.Value)
                                .Cast<Object>()
                                .Select(item => new Node(null, item, null))
                                .ForEach(stack.Push);
                        }
                    }
                }
                else if (currentItem.Parent != null && 
                         currentItem.PropertyInfo != null)
                {
                    sanitizableNodes.Add(currentItem);
                }
            }

            Parallel.ForEach(
                sanitizableNodes,
                _ParallelOptions,
                node =>
                {
                    var nodeValueType = node.PropertyInfo == null
                                            ? node.Value.GetType()
                                            : node.PropertyInfo.PropertyType;

                    if (IsTerminalObject(nodeValueType) && node.Parent != null && node.PropertyInfo != null)
                    {
                        node.PropertyInfo.SetValue(node.Parent, _TextSanitizer.SanitizeHtmlFragment((String)node.Value), null);
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
                            dictionary[key] = _TextSanitizer.SanitizeHtmlFragment((String)dictionary[key]);
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
                                    enumerable[j] = _TextSanitizer.SanitizeHtmlFragment(enumerable[j]);
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
                                    enumerable[j] = _TextSanitizer.SanitizeHtmlFragment((String)enumerable[j]);
                                }
                            }
                        }
                        else if (node.Parent != null && node.PropertyInfo != null)
                        {
                            var sanitizedStringCollection =
                                node.PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>)
                                    ? (ICollection<String>)new List<String>()
                                    : (ICollection<String>)new Collection<String>();

                            ((IEnumerable<String>) node.Value)
                                .ForEach(
                                    value =>
                                    sanitizedStringCollection.Add(String.IsNullOrWhiteSpace(value)
                                                                        ? value
                                                                        : _TextSanitizer.SanitizeHtmlFragment(value)));

                            node.PropertyInfo.SetValue(node.Parent, sanitizedStringCollection, null);
                        }
                    }
                });
        }

        private Boolean IsDictionary(Type type)
        {
            if (type == null) return false;

            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
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

        private class Node : IEquatable<Node>
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

            public static Boolean operator ==(Node x, Node y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (((object)x == null) || ((object)y == null))
                {
                    return false;
                }

                return x.Equals(y);
            }

            public static Boolean operator !=(Node x, Node y)
            {
                return !(x == y);
            }

            public Boolean Equals(Node other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return Equals(_Parent, other._Parent) && Equals(_PropertyInfo, other._PropertyInfo);
            }

            public override Boolean Equals(Object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((Node) obj);
            }

            public override Int32 GetHashCode()
            {
                unchecked
                {
                    var hashCode = (_Value != null ? _Value.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ (_Parent != null ? _Parent.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ (_PropertyInfo != null ? _PropertyInfo.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }
    }
}