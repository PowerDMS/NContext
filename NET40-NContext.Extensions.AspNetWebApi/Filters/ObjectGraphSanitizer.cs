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
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines a object graph traverser - specifically designed for sanitizing user-input.
    /// </summary>
    public class ObjectGraphSanitizer
    {
        private readonly ITextSanitizer _TextSanitizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectGraphSanitizer"/> class.
        /// </summary>
        /// <param name="textSanitizer">The text sanitizer.</param>
        public ObjectGraphSanitizer(ITextSanitizer textSanitizer)
        {
            _TextSanitizer = textSanitizer;
        }

        public void Sanitize(Object objectToSanitize)
        {
            if (objectToSanitize == null || IsTerminalObject(objectToSanitize.GetType()))
            {
                return;
            }

            var idGenerator = new ObjectIDGenerator();
            var stack = new ConcurrentStack<Node>();

            stack.Push(new Node(null, objectToSanitize, null));

            while (!stack.IsEmpty)
            {
                Node currentItem;
                if (!stack.TryPop(out currentItem))
                {
                    break;
                }

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

                var currentItemType = currentItem.Value.GetType();
                if (!IsTerminalObject(currentItemType))
                {
                    if (!(currentItemType.IsGenericType))
                    {
                        Boolean propertyValueHasId;

#if NET40
                        var sanitizableNodes =
                            (from property in currentItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             let propertyValue = property.GetValue(currentItem.Value, null)
                             where (property.PropertyType == typeof(String) && !String.IsNullOrWhiteSpace(propertyValue as String)) ||
                                   (!IsTerminalObject(propertyValue.GetType()) && idGenerator.HasId(propertyValue, out propertyValueHasId) == 0)
                             select new Node(currentItem.Value, property.GetValue(currentItem.Value, null), property)).ToArray();
#elif NET45_OR_GREATER
                        var sanitizableNodes =
                            (from property in currentItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             let propertyValue = property.GetValue(currentItem.Value)
                             where (property.PropertyType == typeof(String) && !String.IsNullOrWhiteSpace(propertyValue as String)) ||
                                   (!IsTerminalObject(propertyValue.GetType()) && idGenerator.HasId(propertyValue, out propertyValueHasId) == 0)
                             select new Node(currentItem.Value, property.GetValue(currentItem.Value), property)).ToArray();
#endif

                        stack.PushRange(sanitizableNodes);
                    }
                    else if (IsEnumerable(currentItemType))
                    {
                        stack.PushRange(
                            ((IEnumerable)currentItem.Value)
                                .Cast<Object>()
                                .Select(item => new Node(currentItem.Parent, item, currentItem.PropertyInfo))
                                .ToArray());
                    }
                }
                else if (currentItem.Parent != null && currentItem.PropertyInfo != null)
                {
#if NET40
                    currentItem.PropertyInfo.SetValue(currentItem.Parent, _TextSanitizer.Sanitize((String)currentItem.Value), null);
#elif NET45_OR_GREATER
                    currentItem.PropertyInfo.SetValue(currentItem.Parent, _TextSanitizer.Sanitize((String)currentItem.Value));
#endif
                }
            }
        }

        private Boolean IsEnumerable(Type type)
        {
            if (type == null) return false;

            return type.GetInterfaces()
                       .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
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
    }
}