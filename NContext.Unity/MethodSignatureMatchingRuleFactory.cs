// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodSignatureMatchingRuleFactory.cs">
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
//
// <summary>
//   Defines a simple factory class for creating a strongly-typed MethodSignatureMatchingRule.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Reflection;

using Microsoft.Practices.Unity.InterceptionExtension;

namespace NContext.Unity
{
    /// <summary>
    /// Defines a simple factory class for creating a strongly-typed <see cref="MethodSignatureMatchingRule"/>.
    /// </summary>
    /// <remarks></remarks>
    // TODO: (DG) Still needed?
    public static class MethodSignatureMatchingRuleFactory
    {
        /// <summary>
        /// Creates a MethodSignatureMatchingRule associated with the specified <see cref="MethodInfo"/>. 
        /// Use the <see cref="Reflect"/> or <see cref="Reflect{TTarget}"/> utility class to resolve the 
        /// desired method signature.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> instance.</param>
        /// <returns>A <see cref="MethodSignatureMatchingRule"/> for policy injection.</returns>
        /// <remarks></remarks>
        public static MethodSignatureMatchingRule Create(MethodInfo methodInfo)
        {
            return new MethodSignatureMatchingRule(methodInfo.Name, methodInfo.GetParameters().Select(p => p.Name));
        }
    }
}