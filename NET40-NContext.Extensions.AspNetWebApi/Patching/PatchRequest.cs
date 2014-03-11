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

namespace NContext.Extensions.AspNetWebApi.Patching
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;

    using NContext.Common;

    /// <summary>
    /// Defines a transfer object for HTTP PATCH support.
    /// </summary>
    public class PatchRequest<TDto> : Dictionary<String, Object>
    {
        /// <summary>
        /// Override the default equality comparer to be case-insensitive.
        /// </summary>
        public PatchRequest() : base(new CaseInsensitiveStringComparer())
        {
        }

        /// <summary>
        /// Patches the specified object using this instance.
        /// TODO: (DG) Refactor this to support patching complex object with navigation properties.
        /// </summary>
        /// <param name="objectToPatch">The object to patch.</param>
        /// <returns>IResponseTransferObject{PatchResult{`0}}.</returns>
        public IResponseTransferObject<PatchResult<TDto>> Patch(TDto objectToPatch)
        {
            var patchableProperties = TypeDescriptor.GetProperties(objectToPatch)
                .Cast<PropertyDescriptor>()
                .Where(prop => prop.Attributes.OfType<WritableAttribute>().Any())
                .ToList();

            var patchOperations = new List<PatchOperation<TDto>>();
            var errors = new List<Error>();

            foreach (var property in patchableProperties)
            {
                if (!ContainsKey(property.Name)) continue;

                var newPropertyValue = this[property.Name];
                var oldPropertyValue = property.GetValue(objectToPatch);

                if (property.PropertyType.IsEnum)
                {
                    newPropertyValue = Enum.Parse(property.PropertyType, newPropertyValue.ToString());
                }

                var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                if (typeConverter.IsValid(newPropertyValue))
                {
                    property.SetValue(objectToPatch, newPropertyValue);
                    patchOperations.Add(new PatchOperation<TDto>(property.Name, oldPropertyValue, newPropertyValue));
                }
                else
                {
                    try
                    {
                        if (property.PropertyType == typeof(DateTime?))
                        {
                            if (newPropertyValue == null || (String.IsNullOrWhiteSpace(newPropertyValue.ToString())))
                            {
                                property.SetValue(objectToPatch, default(DateTime?));
                            }
                            else
                            {
                                property.SetValue(objectToPatch, Convert.ToDateTime(newPropertyValue));
                            }
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(objectToPatch, (Int32) Convert.ChangeType(newPropertyValue, typeof(Int32)));
                        }

                        else if (property.PropertyType == typeof(int?))
                        {
                            int number;
                            Boolean result = Int32.TryParse(newPropertyValue.ToString(), out number);

                            if (result)
                            {
                                property.SetValue(objectToPatch, (Int32?) number);
                            }

                            else
                            {
                                property.SetValue(objectToPatch, null);
                            }
                        }
                        else
                        {
                            property.SetValue(objectToPatch, newPropertyValue);
                        }

                        patchOperations.Add(new PatchOperation<TDto>(property.Name, oldPropertyValue, newPropertyValue));
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ValidationError(typeof(TDto),
                                                       new List<string>
                                                           {
                                                               "Input for the field '" + property.DisplayName +
                                                               "' is invalid."
                                                           }));
                    }
                }
            }

            return errors.Any()
                       ? new ServiceResponse<PatchResult<TDto>>(new AggregateError((Int32)HttpStatusCode.BadRequest, "PatchError", errors))
                       : new ServiceResponse<PatchResult<TDto>>(new PatchResult<TDto>(objectToPatch, patchOperations));
        }

        private class CaseInsensitiveStringComparer : IEqualityComparer<String>
        {
            public Boolean Equals(String x, String y)
            {
                return x.ToUpper().Equals(y.ToUpper());
            }

            public Int32 GetHashCode(String obj)
            {
                return obj.ToUpper().GetHashCode();
            }
        }
    }
}