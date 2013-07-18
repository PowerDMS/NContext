// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFluentValueInjecter.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.ValueInjecter
{
    using System;

    using Omu.ValueInjecter;

    /// <summary>
    /// Defines a fluent, composable way to use ValueInjecter.
    /// </summary>
    public interface IFluentValueInjecter<T>
    {
        /// <summary>
        /// Returns a new instance of <typeparamref name="T2"/>; injecting the source <typeparamref name="T"/> using the 
        /// configured <see cref="IValueInjection"/>. Default <see cref="IValueInjection"/> is <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="T2">The type of target to return.</typeparam>
        /// <returns><typeparamref name="T2"/> instance.</returns>
        T2 Into<T2>() where T2 : class, new();

        /// <summary>
        /// Returns a new instance of <typeparamref name="T2"/>; injecting the source <typeparamref name="T"/> using the 
        /// configured <see cref="IValueInjection"/>. Then applies the custom mapping logic with <paramref name="mapper"/> 
        /// to the result instance. Default <see cref="IValueInjection"/> is <see cref="LoopValueInjection"/>.
        /// </summary>
        /// <typeparam name="T2">The type of target to return.</typeparam>
        /// <returns><typeparamref name="T2"/> instance.</returns>
        T2 Into<T2>(Object mapper) where T2 : class, new();

        /// <summary>
        /// Returns <paramref name="targetInstance" />; injecting the source <typeparamref name="T" /> using the configured
        /// <see cref="IValueInjection" />. Default <see cref="IValueInjection" /> is <see cref="LoopValueInjection" />.
        /// </summary>
        /// <typeparam name="T2">The type of target to return.</typeparam>
        /// <param name="targetInstance">The target instance.</param>
        /// <returns><typeparamref name="T2" /> instance.</returns>
        T2 Into<T2>(T2 targetInstance);

        /// <summary>
        /// Returns <paramref name="targetInstance" />; injecting the source <typeparamref name="T" /> using the configured
        /// <see cref="IValueInjection" />. Then applies the custom mapping logic with <paramref name="mapper" /> to
        /// <paramref name="targetInstance" />. Default <see cref="IValueInjection" /> is <see cref="LoopValueInjection" />.
        /// </summary>
        /// <typeparam name="T2">The type of target to return.</typeparam>
        /// <param name="targetInstance">The target instance.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns><typeparamref name="T2" /> instance.</returns>
        T2 Into<T2>(T2 targetInstance, Object mapper);

        /// <summary>
        /// Configures the type of <see cref="IValueInjection"/> to use with this injector. 
        /// </summary>
        /// <typeparam name="TValueInjection">The type of <see cref="IValueInjection"/>.</typeparam>
        /// <returns>Current <see cref="IFluentValueInjecter{T}"/> instance.</returns>
        IFluentValueInjecter<T> Using<TValueInjection>() where TValueInjection : IValueInjection, new();

        /// <summary>
        /// Configures the <see cref="IValueInjection"/> instance to use with this injector. 
        /// </summary>
        /// <typeparam name="TValueInjection">The type of <see cref="IValueInjection"/>.</typeparam>
        /// <returns>Current <see cref="IFluentValueInjecter{T}"/> instance.</returns>
        IFluentValueInjecter<T> Using<TValueInjection>(TValueInjection valueInjection) where TValueInjection : IValueInjection;
    }
}