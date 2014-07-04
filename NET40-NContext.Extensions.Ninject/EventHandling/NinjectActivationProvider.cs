// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectActivationProvider.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.Ninject.EventHandling
{
    using System;

    using NContext.EventHandling;

    using global::Ninject;

    /// <summary>
    /// Defines an Ninject-based activation provider for event handling.
    /// </summary>
    public class NinjectActivationProvider : IActivationProvider
    {
        private readonly IKernel _Kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectActivationProvider"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public NinjectActivationProvider(IKernel kernel)
        {
            _Kernel = kernel;
        }

        /// <summary>
        /// Creates the handler instance.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns>IHandleEvents.</returns>
        public IHandleEvents CreateInstance<TEvent>(Type handler)
        {
            return _Kernel.Get(handler) as IHandleEvents;
        }
    }
}