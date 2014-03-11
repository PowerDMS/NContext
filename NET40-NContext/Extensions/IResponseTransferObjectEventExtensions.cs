// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseTransferObjectEventExtensions.cs" company="Waking Venture, Inc.">
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
// 
namespace NContext.Extensions
{
    using NContext.Common;
    using NContext.EventHandling;

    /// <summary>
    /// Defines event handling extension methods for <see cref="IResponseTransferObject{T}"/>.
    /// </summary>
    public static class IResponseTransferObjectEventExtensions
    {
        /// <summary>
        /// Raises the specified event. Event handlers may be executed in parallel. All handlers 
        /// are run even if one throws an exception. If an exception is thrown, this method returns
        /// a new <see cref="ServiceResponse{T}"/> with errors representing the thrown exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEvent">The type of the T event.</typeparam>
        /// <param name="responseTransferObject">The response transfer object.</param>
        /// <param name="event">The event.</param>
        public static IResponseTransferObject<T> Raise<T, TEvent>(this IResponseTransferObject<T> responseTransferObject, TEvent @event)
        {
            return EventManager.RaiseEvent<TEvent>(@event)
                               .ContinueWith(task => task.IsFaulted
                                                         ? new ServiceResponse<T>(task.Exception.ToError())
                                                         : responseTransferObject).Result;
        }
    }
}