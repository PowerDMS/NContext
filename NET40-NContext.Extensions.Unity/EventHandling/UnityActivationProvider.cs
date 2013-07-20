// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityActivationProvider.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.Unity.EventHandling
{
    using System;

    using Microsoft.Practices.Unity;

    using NContext.EventHandling;

    /// <summary>
    /// Defines an Unity-based activation provider for event handling.
    /// </summary>
    public class UnityActivationProvider : IActivationProvider
    {
        private readonly IUnityContainer _Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityActivationProvider"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public UnityActivationProvider(IUnityContainer container)
        {
            _Container = container;
        }

        /// <summary>
        /// Creates the handler instance.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns>IHandleEvent{TEvent}.</returns>
        public IHandleEvent<TEvent> CreateInstance<TEvent>(Type handler)
        {
            return _Container.Resolve(handler) as IHandleEvent<TEvent>;
        }
    }
}