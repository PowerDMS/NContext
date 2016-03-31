namespace NContext.Extensions
{
    using System;
    using System.ComponentModel;

    using NContext.Utilities;

    /// <summary>
    /// Defines a static class for providing enum type extension methods.
    /// </summary>
    /// <remarks></remarks>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the <see cref="DescriptionAttribute"/> value based upon the enum value specified.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of the <see cref="DescriptionAttribute"/> if one exists, else <see cref="string.Empty"/>.</returns>
        /// <remarks></remarks>
        public static String ToDescriptionValue(this Enum value)
        {
            return AttributeUtility.GetDescriptionAttributeValueFromField(value);
        }
    }
}