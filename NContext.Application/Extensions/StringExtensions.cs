// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs">
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
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.// </copyright>
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
        /// Returns the number of <see cref="string.Format(string,object)"/> parameters in the specified text.
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