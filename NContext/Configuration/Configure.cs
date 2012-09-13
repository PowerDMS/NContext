// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configure.cs" company="Waking Venture, Inc.">
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

namespace NContext.Configuration
{
    using System;

    /// <summary>
    /// Defines a static class for fluid-interface application configuration.
    /// </summary>
    /// <remarks></remarks>
    public static class Configure
    {
        /// <summary>
        /// Configures the application using the specified <see cref="ApplicationConfigurationBase"/> instance.
        /// </summary>
        /// <typeparam name="TApplicationConfiguration">The type of the application configuration.</typeparam>
        /// <param name="applicationConfiguration">The application configuration instance.</param>
        /// <remarks></remarks>
        public static void Using<TApplicationConfiguration>(TApplicationConfiguration applicationConfiguration) 
            where TApplicationConfiguration : ApplicationConfigurationBase
        {
            if (applicationConfiguration == null)
            {
                throw new ArgumentNullException("applicationConfiguration");
            }

            applicationConfiguration.Setup();
        }
    }
}