// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBaseTests.cs">
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
//   Defines unit tests for application error handling.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using NContext.ErrorHandling;

using NUnit.Framework;

namespace NContext.Tests.Unit.ErrorHandling
{
    /// <summary>
    /// Defines unit tests for application error handling.
    /// </summary>
    [TestFixture]
    public class ErrorBaseTests
    {
        public sealed class MockApplicationError : ErrorBase
        {
            private MockApplicationError(String localizationKey, params Object[] errorMessageParameters)
                : base(localizationKey, errorMessageParameters)
            {
            }

            private MockApplicationError(String localizationKey, HttpStatusCode httpStatusCode, params Object[] errorMessageParameters)
                : base(localizationKey, httpStatusCode, errorMessageParameters)
            {
            }

            /// <summary>
            /// Returns "This is a localized error message."
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public static MockApplicationError BasicError()
            {
                return new MockApplicationError("BasicError", HttpStatusCode.Unauthorized);
            }

            /// <summary>
            /// Returns "Parameters are used for <param name="paramInt"></param> identifiers which are <param name="paramString"></param>-agnostic."
            /// </summary>
            public static MockApplicationError ErrorWithParameters(Int32 paramInt, String paramString)
            {
                return new MockApplicationError("ErrorWithParameters", paramInt, paramString);
            }

            /// <summary>
            /// Throws a <see cref="FormatException"/>.
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public static MockApplicationError ErrorWithIncorrectParameters()
            {
                return new MockApplicationError("ErrorWithParameters");
            }

            /// <summary>
            /// Returns ""
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public static MockApplicationError ErrorWithNoKeyInResource()
            {
                return new MockApplicationError("ErrorWithNoKeyInResource");
            }
        }

        [Test]
        public void Message_NoLocalizationKeyInResource_ReturnsAnEmptyString()
        {
            var error = MockApplicationError.ErrorWithNoKeyInResource();

            Assert.That(error.Message, Is.Empty);
        }

        [Test]
        public void Message_KeyInResource_ReturnsLocalizedMessage()
        {
            var error = MockApplicationError.BasicError();

            Assert.That(error.Message, Is.EqualTo("This is a localized error message."));
        }

        [Test]
        public void Message_StringWithFormatters_ReturnsLocalizedAndFormattedMessage()
        {
            const String lang = "language";
            var error = MockApplicationError.ErrorWithParameters(5, lang);

            Assert.That(error.Message, Is.EqualTo("Parameters are used for 5 identifiers which are language-agnostic."));
        }

        [Test]
        public void Message_CultureResourceExists_ReturnsALocalizedCultureSpecificMessage()
        {
            var errorTask = new Task<MockApplicationError>(() =>
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES", false);
                    return MockApplicationError.BasicError();
                });

            errorTask.Start();

            Assert.That(errorTask.Result.Message, Is.EqualTo("Este es un mensaje de error localizado."));
        }

        [Test]
        public void Message_CultureResourceDoesNotExist_ReturnsDefaultCultureMessage()
        {
            var errorTask = new Task<MockApplicationError>(() =>
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("da", false);
                return MockApplicationError.BasicError();
            });

            errorTask.Start();

            Assert.That(errorTask.Result.Message, Is.EqualTo("This is a localized error message."));
        }

        [Test]
        public void GetLocalizedErrorMessage_IncorrectFormatParametersSpecified_ThrowsAnException()
        {
            var error = new TestDelegate(() => MockApplicationError.ErrorWithIncorrectParameters());

            Assert.That(error, Throws.TypeOf<FormatException>());
        }
    }
}