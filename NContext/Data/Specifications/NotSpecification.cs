//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotSpecification.cs">
//   Copyright (c) 2012
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
//   Defines a specification which converts an original specification with inverse logic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

namespace NContext.Data.Specifications
{
    /// <summary>
    /// Defines a specification which converts an original specification with inverse logic.
    /// </summary>
    /// <typeparam name="TEntity">Type of element for this specificaiton</typeparam>
    public sealed class NotSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        #region Fields

        private readonly Expression<Func<TEntity, Boolean>> _OriginalCriteria;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="originalSpecification">The original specification.</param>
        /// <remarks></remarks>
        public NotSpecification(SpecificationBase<TEntity> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException("originalSpecification");
            }

            _OriginalCriteria = originalSpecification.IsSatisfiedBy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="originalSpecification">The original specification.</param>
        /// <remarks></remarks>
        public NotSpecification(Expression<Func<TEntity, Boolean>> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException("originalSpecification");
            }

            _OriginalCriteria = originalSpecification;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfiedBy()
        {
            return Expression.Lambda<Func<TEntity, Boolean>>(Expression.Not(_OriginalCriteria.Body), _OriginalCriteria.Parameters.Single());
        }

        #endregion
    }
}