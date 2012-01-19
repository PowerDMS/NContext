// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs">
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
//   Defines a static class for providing String type extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace NContext.Application.Extensions
{
    /// <summary>
    /// Defines a static class for providing String type extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the number of <see cref="String.Format(String,Object[])"/> parameters in the specified text.
        /// </summary>
        /// <param name="text">The text to scan.</param>
        /// <returns>Number of required String.Format parameters.</returns>
        public static Int32 MinimumFormatParametersRequired(this String text)
        {
            Int32 counter = -1;
            foreach (Match match in Regex.Matches(text, @"{(\d+)}+", RegexOptions.IgnoreCase))
            {
                Int32 temp = Int32.Parse(match.Groups[1].ToString());
                if (temp > counter)
                {
                    counter = temp;
                }
            }

            return ++counter;
        }

        /// <summary>
        /// Splits a string into a NameValueCollection, where each "namevalue" is separated by
        /// the "OuterSeparator". The parameter "NameValueSeparator" sets the split between Name and Value.
        /// Example: 
        ///             String str = "param1=value1;param2=value2";
        ///             NameValueCollection nvOut = str.ToNameValueCollection(';', '=');
        ///             
        /// The result is a NameValueCollection where:
        ///             key[0] is "param1" and value[0] is "value1"
        ///             key[1] is "param2" and value[1] is "value2"
        /// </summary>
        /// <param name="str">String to process</param>
        /// <param name="OuterSeparator">Separator for each "NameValue"</param>
        /// <param name="NameValueSeparator">Separator for Name/Value splitting</param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(this String str, Char OuterSeparator, Char NameValueSeparator)
        {
            NameValueCollection nvText = null;
            str = str.TrimEnd(OuterSeparator);
            if (!String.IsNullOrEmpty(str))
            {
                String[] arrStrings = str.TrimEnd(OuterSeparator).Split(OuterSeparator);

                foreach (String nameValuePair in arrStrings)
                {
                    Int32 posSep = nameValuePair.IndexOf(NameValueSeparator);
                    String name = nameValuePair.Substring(0, posSep);
                    String value = nameValuePair.Substring(posSep + 1).Trim(new [] { '"' });
                    if (nvText == null)
                    {
                        nvText = new NameValueCollection();
                    }

                    nvText.Add(name, value);
                }
            }

            return nvText;
        }
    }
}