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
// <copyright file="AdHocSpecification.cs">
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
//   Defines a specification which is composed AdHoc via constructor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace NContext.Data.Specifications
{
    using NContext.Data.Persistence;

    /// <summary>
    /// Defines a specification which is composed AdHoc via constructor.
    /// </summary>
    /// <typeparam name = "TEntity">Type of entity that check this specification</typeparam>
    public sealed class AdHocSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        #region Fields

        private readonly Expression<Func<TEntity, Boolean>> _MatchingCriteria;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdHocSpecification{TEntity}"/> class.
        /// </summary>
        /// <param name="matchingCriteria">The matching criteria.</param>
        /// <remarks></remarks>
        public AdHocSpecification(Expression<Func<TEntity, Boolean>> matchingCriteria)
        {
            if (matchingCriteria == null)
            {
                throw new ArgumentNullException("matchingCriteria");
            }

            _MatchingCriteria = matchingCriteria;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean expression which determines whether the specification is satisfied.
        /// </summary>
        /// <returns>Expression that evaluates whether the specification satifies the expression.</returns>
        public override Expression<Func<TEntity, Boolean>> IsSatisfiedBy()
        {
            return _MatchingCriteria;
        }

        #endregion
    }
}