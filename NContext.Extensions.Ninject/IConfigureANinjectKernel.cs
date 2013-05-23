// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigureANinjectKernel.cs" company="Waking Venture, Inc.">
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

namespace NContext.Extensions.Ninject
{
    using System;
    using System.ComponentModel.Composition;

    using global::Ninject;

    /// <summary>
    /// Defines a role-interface for Ninject's IKernel configuration.
    /// </summary>
    [InheritedExport]
    public interface IConfigureANinjectKernel
    {
        /// <summary>
        /// Configures the <see cref="IKernel"/> instance.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <remarks></remarks>
        void ConfigureKernel(IKernel kernel);

        /// <summary>
        /// Get's the priority used to configure the <see cref="IKernel"/>. Lower 
        /// priority will cause implementations to execute first.
        /// </summary>
        Int32 Priority { get; }
    }
}