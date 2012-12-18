// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBaseExtensions.cs" company="Waking Venture, Inc.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions
{
    using Microsoft.FSharp.Core;

    using NContext.Common;
    using NContext.ErrorHandling;

    /// <summary>
    /// Defines extension methods for <see cref="ErrorBase"/>.
    /// </summary>
    public static class ErrorBaseExtensions
    {
        /// <summary>
        /// Returns a new <see cref="IResponseTransferObject{Unit}"/> with the specified <paramref name="error"/>.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>IResponseTransferObject{Unit}.</returns>
        public static IResponseTransferObject<Unit> ToServiceResponse(this ErrorBase error)
        {
            return new ServiceResponse<Unit>(error);
        }

        /// <summary>
        /// Returns a new <see cref="IResponseTransferObject{T}"/> with the specified <paramref name="error"/>.
        /// </summary>
        /// <typeparam name="T">Type of IResponseTransferObject.</typeparam>
        /// <param name="error">The error.</param>
        /// <returns>IResponseTransferObject{T}.</returns>
        public static IResponseTransferObject<T> ToServiceResponse<T>(this ErrorBase error)
        {
            return new ServiceResponse<T>(error);
        }
    }
}