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
// <copyright file="AndSpecification.cs">
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
//   Defines a composite specification for AND-logic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace NContext.Extensions.EntityFramework.Specifications
{
    /// <summary>
    /// Defines a composite specification for AND-logic.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification</typeparam>
    public sealed class AndSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : class, IEntity
    {
        #region Fields

        private readonly SpecificationBase<TEntity> _LeftSideSpecification;

        private readonly SpecificationBase<TEntity> _RightSideSpecification;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="leftSide">The left side.</param>
        /// <param name="rightSide">The right side.</param>
        /// <remarks></remarks>
        public AndSpecification(SpecificationBase<TEntity> leftSide, SpecificationBase<TEntity> rightSide)
        {
            if (leftSide == null)
            {
                throw new ArgumentNullException("leftSide");
            }

            if (rightSide == null)
            {
                throw new ArgumentNullException("rightSide");
            }

            _LeftSideSpecification = leftSide;
            _RightSideSpecification = rightSide;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Left side specification
        /// </summary>
        public override SpecificationBase<TEntity> LeftSideSpecification
        {
            get { return _LeftSideSpecification; }
        }

        /// <summary>
        /// Right side specification
        /// </summary>
        public override SpecificationBase<TEntity> RightSideSpecification
        {
            get { return _RightSideSpecification; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfiedBy()
        {
            Expression<Func<TEntity, Boolean>> left = _LeftSideSpecification.IsSatisfiedBy();
            Expression<Func<TEntity, Boolean>> right = _RightSideSpecification.IsSatisfiedBy();

            return (left.And(right));
        }

        #endregion
    }
}