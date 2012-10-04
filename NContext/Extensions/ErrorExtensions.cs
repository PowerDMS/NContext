// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorExtensions.cs" company="Waking Venture, Inc.">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using NContext.Common;
    using NContext.ErrorHandling;

    /// <summary>
    /// Defines extension methods for <see cref="Error"/>.
    /// </summary>
    public static class ErrorExtensions
    {
        /// <summary>
        /// Return a string of all the error messages seperated by new lines.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns>String.</returns>
        public static String ToMessage(this IEnumerable<Error> errors)
        {
            var stringBuilder = new StringBuilder();
            errors.SelectMany(error => error.Messages)
                  .ForEach(message => stringBuilder.AppendLine(message));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns the <paramref name="error"/> as a <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="error">The error to convert.</param>
        /// <returns><typeparamref name="TException"/> instance.</returns>
        /// <exception cref="TargetInvocationException">
        /// Thrown when <typeparamref name="TException"/> does not have a constructor which takes in an exception message <see cref="String"/>.
        /// </exception>
        public static TException ToException<TException>(this ErrorBase error)
            where TException : Exception
        {
            return (TException)Activator.CreateInstance(typeof(TException), error.Message);
        }

        /// <summary>
        /// Returns the <paramref name="error" /> as a <typeparamref name="TException" />.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="error">The error to convert.</param>
        /// <param name="exceptionFactory">The exception factory.</param>
        /// <returns><typeparamref name="TException" /> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="exceptionFactory" /> is null.</exception>
        public static TException ToException<TException>(this ErrorBase error, Func<ErrorBase, TException> exceptionFactory)
            where TException : Exception
        {
            if (exceptionFactory == null)
            {
                throw new ArgumentNullException("exceptionFactory");
            }

            return exceptionFactory.Invoke(error);
        }
    }
}