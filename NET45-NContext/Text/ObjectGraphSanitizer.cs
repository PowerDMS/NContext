namespace NContext.Text
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    using NContext.Common;
    using NContext.Common.Text;
    using NContext.Exceptions;
    using NContext.Extensions;

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
        public virtual void Sanitize(Object objectToSanitize)
        {
            if (objectToSanitize == null || IsTerminalObject(objectToSanitize.GetType()))
            {
                return;
            }

            var idGenerator = new ObjectIDGenerator();
            var stack = new Stack<Node>();
            var sanitizableNodes = new HashSet<Node>();

            stack.Push(new Node(null, objectToSanitize, null, false));

            while (stack.Count > 0)
            {
                Node currentItem = stack.Pop();

                if (currentItem.Value == null)
                    continue;

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

                var currentItemType = currentItem.PropertyInfo == null 
                    ? currentItem.Value.GetType() 
                    : currentItem.PropertyInfo.PropertyType;

                if (!IsTerminalObject(currentItemType))
                {
                    if (IsDictionary(currentItemType))
                    {
                        var genericDictionaryInterface =
                            currentItemType.IsGenericType &&
                            currentItemType.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                                ? currentItemType
                                : currentItemType.GetInterfaces()
                                    .SingleOrDefault(
                                        interfaceType =>
                                            interfaceType.IsGenericType &&
                                            interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>));

                        if (genericDictionaryInterface != null)
                        {
                            ProcessGenericDictionary(genericDictionaryInterface, sanitizableNodes, currentItem, currentItemType, stack);
                            continue;
                        }

                        throw new NotSupportedException("ObjectGraphSanitizer does not support non-generic dictionaries.");
                    }

                    if (IsEnumerable(currentItemType))
                    {
                        var genericEnumerableInterface =
                            currentItemType.IsGenericType &&
                            currentItemType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                                ? currentItemType
                                : currentItemType.GetInterfaces()
                                    .SingleOrDefault(
                                        interfaceType =>
                                            interfaceType.IsGenericType &&
                                            interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                        if (genericEnumerableInterface != null)
                        {
                            ProcessGenericEnumerable(genericEnumerableInterface, sanitizableNodes, currentItem, stack);
                            continue;
                        }

                        throw new NotSupportedException("ObjectGraphSanitizer does not support non-generic enumerables.");
                    }

                    (from property in currentItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        let propertyValue = property.GetValue(currentItem.Value, null)
                        let allowHtml = property
                            .GetCustomAttributes(typeof(SanitizationHtmlAttribute), true)
                            .SingleOrDefault().ToMaybe().Bind(_ => true.ToMaybe()).FromMaybe(false)
                        where PropertyIsSanitizable(property, propertyValue, idGenerator)
                        select new Node(currentItem.Value, propertyValue, property, allowHtml))
                        .ForEach(stack.Push);

                    continue;
                }

                if (currentItem.Parent != null && currentItem.PropertyInfo != null)
                {
                    sanitizableNodes.Add(currentItem);
                    continue;
                }

                throw new SanitizationException(String.Format("ObjectGraphSanitizer could not handle the object type: {0}", currentItemType));
            }

            SanitizeNodes(sanitizableNodes);
        }

        private bool PropertyIsSanitizable(PropertyInfo property, object propertyValue, ObjectIDGenerator idGenerator)
        {
            /* Get all properties that are either:
             * a) strings that aren't null or whitespace
             * b) uris that aren't null
             * c) non-terminal objects that we haven't already visited in the graph.
             * d) properties without the SanitizationIgnoreAttribute
             * */

            bool propertyValueHasId;
            return property.GetCustomAttributes(typeof(SanitizationIgnoreAttribute), true)
                .SingleOrDefault()
                .ToMaybe()
                .Bind(_ => false.ToMaybe())
                .FromMaybe(true) &&
                   ((property.PropertyType == typeof(String) && !String.IsNullOrWhiteSpace(propertyValue as String)) ||
                    (property.PropertyType == typeof(Uri) && propertyValue != null) ||
                    (propertyValue != null && 
                    !IsTerminalObject(propertyValue.GetType()) &&
                     idGenerator.HasId(propertyValue, out propertyValueHasId) == 0));
        }

        private void ProcessGenericEnumerable(
            Type genericEnumerableInterface, 
            HashSet<Node> sanitizableNodes, 
            Node currentItem,
            Stack<Node> stack)
        {
            var genericArgumentType = genericEnumerableInterface.GetGenericArguments().Single();
            if (genericArgumentType == typeof (String))
            {
                sanitizableNodes.Add(currentItem);
            }
            else if (genericArgumentType == typeof (Object))
            {
                // BUG: (DG) You can't assume all objects that implement IEnumerable<> also implement IEnumerable
                sanitizableNodes.Add(currentItem);
                ((IEnumerable) currentItem.Value)
                    .Cast<Object>()
                    .Where(element => element != null && !IsTerminalObject(element.GetType()))
                    .Select(element => new Node(null, element, null, currentItem.AllowHtml))
                    .ForEach(stack.Push);
            }
            else if (!IsTerminalObject(genericArgumentType))
            {
                ((IEnumerable) currentItem.Value)
                    .Cast<Object>()
                    .Select(item => new Node(null, item, null, currentItem.AllowHtml))
                    .ForEach(stack.Push);
            }
            else
            {
                throw new SanitizationException(
                    String.Format(
                        "ObjectGraphSanitizer could not handle the object type: {0}",
                        genericArgumentType.FullName));
            }
        }

        private void ProcessGenericDictionary(
            Type genericDictionaryInterface, 
            HashSet<Node> sanitizableNodes, 
            Node currentItem,
            Type currentItemType, 
            Stack<Node> stack)
        {
            var genericDictionaryValueType = genericDictionaryInterface.GetGenericArguments().Last();
            if (genericDictionaryValueType == typeof (String))
            {
                sanitizableNodes.Add(currentItem);
                return;
            }
            
            if (genericDictionaryValueType == typeof (Object))
            {
                sanitizableNodes.Add(currentItem);
                if (currentItemType.Implements<IDictionary>())
                {
                    var dictionary = ((IDictionary)currentItem.Value);
                    var enumerator = dictionary.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Value != null && !IsTerminalObject(enumerator.Value.GetType()))
                        {
                            stack.Push(new Node(null, enumerator.Value, null, currentItem.AllowHtml));
                        }
                    }

                    return;
                }

                if (currentItem.Value != null)
                {
                    throw new NotSupportedException(
                        String.Format("ObjectGraphSanitizer does not support dictionaries that do not implement IDictionary. Type '{0}' is unsupported.", currentItemType));
                }
            }

            if (!IsTerminalObject(genericDictionaryValueType))
            {
                if (currentItemType.Implements<IDictionary>())
                {
                    var dictionary = ((IDictionary) currentItem.Value);
                    var enumerator = dictionary.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        stack.Push(new Node(currentItem.Parent, enumerator.Value, currentItem.PropertyInfo, currentItem.AllowHtml));
                    }

                    return;
                }

                if (currentItem.Value != null)
                {
                    throw new SanitizationException(
                        String.Format(
                            "ObjectGraphSanitizer could not handle the object type: {0}",
                            genericDictionaryValueType.FullName));
                }
            }

            throw new SanitizationException(
                String.Format(
                    "ObjectGraphSanitizer could not handle the object type: {0}",
                    genericDictionaryValueType.FullName));
        }

        protected virtual void SanitizeNodes(ISet<Node> sanitizableNodes)
        {
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
                        bool isUri = node.PropertyInfo.PropertyType == typeof(Uri);
                        var value = isUri
                            ? node.Value.ToString()
                            : (String)node.Value;

                        var sanitizedValue = node.AllowHtml
                            ? _TextSanitizer.SanitizeHtml(value)
                            : _TextSanitizer.SanitizeHtmlFragment(value);

                        var newValue = isUri
                            ? new Uri(sanitizedValue)
                            : (Object)sanitizedValue;

                        node.PropertyInfo.SetValue(
                            node.Parent,
                            newValue,
                            null);

                        return;
                    }

                    if (IsDictionary(nodeValueType))
                    {
                        var dictionary = (IDictionary)node.Value;
                        var keyEnumerator = dictionary.Keys.Cast<object>().ToList().GetEnumerator();
                        while (keyEnumerator.MoveNext())
                        {
                            var entry = keyEnumerator.Current;
                            var value = dictionary[entry];
                            if (value == null || !(!String.IsNullOrWhiteSpace(value as string) || value is Uri))
                                continue;

                            var isUri = false;
                            if (value is Uri)
                            {
                                isUri = true;
                                value = value.ToString();
                            }

                            var sanitizedValue = node.AllowHtml
                                ? _TextSanitizer.SanitizeHtml((String)value)
                                : _TextSanitizer.SanitizeHtmlFragment((String)value);

                            object dictValue = isUri
                                ? (object)new Uri(sanitizedValue)
                                : (object)sanitizedValue;

                            dictionary[entry] = dictValue;
                        }

                        return;
                    }

                    // IList<string>
                    if (node.Parent == null && node.PropertyInfo == null && node.Value as IList<String> != null)
                    {
                        var enumerable = (IList<String>)node.Value;
                        for (var j = 0; j < enumerable.Count; j++)
                        {
                            if (!String.IsNullOrWhiteSpace(enumerable[j]))
                            {
                                enumerable[j] = node.AllowHtml
                                ? _TextSanitizer.SanitizeHtml(enumerable[j])
                                : _TextSanitizer.SanitizeHtmlFragment(enumerable[j]);
                            }
                        }

                        return;
                    }

                    // IList<object>
                    if (node.Parent == null && node.PropertyInfo == null && node.Value as IList<Object> != null)
                    {
                        var enumerable = (IList<Object>)node.Value;
                        for (var j = 0; j < enumerable.Count; j++)
                        {
                            if (enumerable[j] is String && !String.IsNullOrWhiteSpace(enumerable[j] as String))
                            {
                                enumerable[j] = node.AllowHtml
                                ? _TextSanitizer.SanitizeHtml((String)enumerable[j])
                                : _TextSanitizer.SanitizeHtmlFragment((String)enumerable[j]);
                            }
                            else if (enumerable[j] is Uri && enumerable[j] != null)
                            {
                                var uriValue = enumerable[j].ToString();
                                enumerable[j] = node.AllowHtml
                                ? new Uri(_TextSanitizer.SanitizeHtml(uriValue))
                                : new Uri(_TextSanitizer.SanitizeHtmlFragment(uriValue));
                            }
                        }

                        return;
                    }

                    if (node.Parent != null && node.PropertyInfo != null)
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
                                        : node.AllowHtml
                                            ? _TextSanitizer.SanitizeHtml(value)
                                            : _TextSanitizer.SanitizeHtmlFragment(value)));

                        node.PropertyInfo.SetValue(node.Parent, sanitizedStringCollection, null);

                        return;
                    }

                    throw new SanitizationException(String.Format("ObjectGraphSanitizer was unable to sanitize node: {0}", node.ToString()));
                });
        }

        private Boolean IsDictionary(Type type)
        {
            if (type == null) return false;

            return  (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
                   type.GetInterfaces()
                       .Any(interfaceType => (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>)) ||
                                             (interfaceType == typeof(IDictionary)));
        }

        private Boolean IsEnumerable(Type type)
        {
            if (type == null) return false;

            return 
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) || 
                type.GetInterfaces()
                    .Any(interfaceType => (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                                          (interfaceType == typeof(IEnumerable)));
        }

        private Boolean IsTerminalObject(Type type)
        {
            if (type == null) return true;

            return type.IsPrimitive || 
                   type.IsEnum || 
                   type.IsPointer ||
                   type == typeof(String) ||
                   type == typeof(DateTime) ||
                   type == typeof(Uri) ||
                   type == typeof(Decimal) ||
                   type == typeof(Guid) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan);
        }

        protected class Node : IEquatable<Node>
        {
            private readonly Object _Parent;

            private readonly Object _Value;

            private readonly PropertyInfo _PropertyInfo;

            private readonly Boolean _AllowHtml;

            public Node(Object parent, Object value, PropertyInfo propertyInfo, Boolean allowHtml)
            {
                _Parent = parent;
                _PropertyInfo = propertyInfo;
                _AllowHtml = allowHtml;
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

            public Boolean AllowHtml
            {
                get { return _AllowHtml; }
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

            public override string ToString()
            {
                return String.Format(
                    "ParentInstanceType: {0}, PropertyName: {1}, PropertyValue {2}",
                    Parent.GetType().FullName,
                    PropertyInfo.Name,
                    Value);
            }
        }
    }
}