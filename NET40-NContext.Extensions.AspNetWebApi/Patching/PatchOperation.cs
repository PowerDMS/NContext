// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatchOperation.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.AspNetWebApi.Patching
{
    using System;
    using System.Linq.Expressions;

    public class PatchOperation<TDto>
    {
        private readonly String _PropertyName;

        private readonly Object _OldValue;

        private readonly Object _NewValue;

        public PatchOperation(String propertyName, Object oldValue, Object newValue)
        {
            _PropertyName = propertyName;
            _OldValue = oldValue;
            _NewValue = newValue;
        }

        public String PropertyName
        {
            get
            {
                return _PropertyName;
            }
        }

        public Object OldValue
        {
            get
            {
                return _OldValue;
            }
        }

        public Object NewValue
        {
            get
            {
                return _NewValue;
            }
        }

        public Boolean HasChangedTo(Object value)
        {
            return NewValue.Equals(value) && !OldValue.Equals(value);
        }

        public Boolean IsProperty<TProperty>(Expression<Func<TDto, TProperty>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression)
                .Member
                .Name
                .Equals(PropertyName, StringComparison.OrdinalIgnoreCase);
        }
    }
}