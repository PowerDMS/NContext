// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberAccessPathVisitor.cs">
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

// <summary>
//   Summary description for MemberAccessPathVisitor
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NContext.Persistence
{
    /// <summary>
    /// Inherits from the <see cref="ExpressionVisitor"/> base class and implements a expression visitor
    /// that builds up a path string that represents meber access in a Expression.
    /// </summary>
    public class MemberAccessPathVisitor : ExpressionTreeVisitor
    {
        private readonly LinkedList<String> _Path = new LinkedList<String>();

        /// <summary>
        /// Gets the path analyzed by the visitor.
        /// </summary>
        public String Path
        {
            get
            {
                var pathString = new StringBuilder();
                foreach (String path in _Path)
                {
                    if (pathString.Length == 0)
                    {
                        pathString.Append(path);
                    }
                    else
                    {
                        pathString.AppendFormat(".{0}", path);
                    }
                }

                return pathString.ToString();
            }
        }

        /// <summary>
        /// Overrides all MemberAccess to build a path string.
        /// </summary>
        /// <param name="memberExpression">The method exp.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected override Expression VisitMemberAccess(MemberExpression memberExpression)
        {
            if (memberExpression.Member.MemberType != MemberTypes.Field && memberExpression.Member.MemberType != MemberTypes.Property)
            {
                throw new NotSupportedException(String.Format("MemberAccessPathVisitor does not support a member access of type {0}.", memberExpression.Member.MemberType));
            }

            _Path.AddFirst(memberExpression.Member.Name);

            return base.VisitMemberAccess(memberExpression);
        }

        /// <summary>
        /// Overriden. Throws a <see cref="NotSupportedException"/> when a method call is encountered.
        /// </summary>
        /// <param name="methodCallExpression">The method call expression.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            throw new NotSupportedException("MemberAccessPathVisitor does not support method calls. Only member expressions are allowed.");
        }
    }
}