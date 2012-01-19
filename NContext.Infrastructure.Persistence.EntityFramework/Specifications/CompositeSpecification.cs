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
// <copyright file="CompositeSpecification.cs">
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
//   Defines a base abstraction for creating composite specifications.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Infrastructure.Persistence.EntityFramework.Specifications
{
    /// <summary>
    /// Defines a base abstraction for creating composite specifications.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public abstract class CompositeSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class, IEntity
    {
        #region Properties

        /// <summary>
        /// Gets the left side specification for this composite element.
        /// </summary>
        /// <remarks></remarks>
        public abstract SpecificationBase<TEntity> LeftSideSpecification { get; }

        /// <summary>
        /// Gets the right side specification for this composite element.
        /// </summary>
        public abstract SpecificationBase<TEntity> RightSideSpecification { get; }

        #endregion
    }
}