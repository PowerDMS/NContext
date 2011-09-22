// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodSignatureMatchingRuleFactory.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//
// <summary>
//   Defines a simple factory class for creating a strongly-typed MethodSignatureMatchingRule.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Reflection;

using Microsoft.Practices.Unity.InterceptionExtension;

using NContext.Application.Utilities;

namespace NContext.Unity
{
    /// <summary>
    /// Defines a simple factory class for creating a strongly-typed <see cref="MethodSignatureMatchingRule"/>.
    /// </summary>
    /// <remarks></remarks>
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