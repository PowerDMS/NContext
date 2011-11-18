// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBaseTests.cs">
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
//   Defines unit tests for application error handling.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using NContext.Application.ErrorHandling;

using NUnit.Framework;

namespace NContext.Application.Tests.Unit.ErrorHandling
{
    /// <summary>
    /// Defines unit tests for application error handling.
    /// </summary>
    [TestFixture]
    public class ErrorBaseTests
    {
        private enum MockError
        {
            DefaultError,

            [Error(LocalizationKey = "KeyOverride")]
            ErrorWithSpecificKey,

            /// <summary>
            /// Parameters are used for {0} which are {1}-agnostic.
            /// </summary>
            ErrorWithParameters,

            ErrorWithNoKeyInResource
        }

        private class MockApplicationError : ErrorBase
        {
            public MockApplicationError(MockError error, params Object[] errorMessageParameters)
                : base(typeof(MockError), error.ToString(), errorMessageParameters)
            {
            }

            protected override Type LocalizationResource
            {
                get
                {
                    return typeof(Localization.MockError);
                }
            }
        }

        [Test]
        public void MockError_DefaultLocalizationKey_ShouldReturnLocalizedMessage()
        {
            var error = new MockApplicationError(MockError.DefaultError);
            Assert.That(error.Message, Is.EqualTo("This is a localized error message."));
        }

        [Test]
        public void MockError_SpecifiedLocalizationKey_ShouldReturnLocalizedMessage()
        {
            var error = new MockApplicationError(MockError.ErrorWithSpecificKey);
            Assert.That(error.Message, Is.EqualTo("This is a localized error message."));
        }

        [Test]
        public void MockError_CustomParameters_ShouldReturnFormattedLocalizedMessage()
        {
            const String lang = "language";
            var error = new MockApplicationError(MockError.ErrorWithParameters, "identifiers", lang);
            Assert.That(error.Message, Is.EqualTo("Parameters are used for identifiers which are language-agnostic."));
        }

        [Test]
        public void MockError_NoKeyInResource_ShouldReturnEmptyMessage()
        {
            var error = new MockApplicationError(MockError.ErrorWithNoKeyInResource);
            Assert.That(error.Message, Is.Empty);
        }
    }
}