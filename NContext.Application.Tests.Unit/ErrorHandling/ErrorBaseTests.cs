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
        public void Should_return_an_empty_message_when_localization_key_does_not_exist()
        {
            var error = MockApplicationError.ErrorWithNoKeyInResource();

            Assert.That(error.Message, Is.Empty);
        }

        [Test]
        public void Should_return_a_localized_message()
        {
            var error = MockApplicationError.BasicError();

            Assert.That(error.Message, Is.EqualTo("This is a localized error message."));
        }

        [Test]
        public void Should_return_a_string_formatted_localized_message_when_specifying_parameters()
        {
            const String lang = "language";
            var error = MockApplicationError.ErrorWithParameters(5, lang);

            Assert.That(error.Message, Is.EqualTo("Parameters are used for 5 identifiers which are language-agnostic."));
        }

        [Test]
        public void Should_return_a_localized_culture_specific_message()
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
        public void Should_throw_an_exception_when_incorrect_format_parameters_specified()
        {
            var error = new TestDelegate(() => MockApplicationError.ErrorWithIncorrectParameters());

            Assert.That(error, Throws.TypeOf<FormatException>());
        }
        
        [Test]
        public void Should_set_the_http_status_code_when_specified()
        {
            var error = MockApplicationError.BasicError();

            Assert.That(error.HttpStatusCode == HttpStatusCode.Unauthorized);
        }
    }
}