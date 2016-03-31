namespace NContext.Utilities
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Defines helper methods for using attributes and reflection.
    /// </summary>
    public sealed class AttributeUtility
    {
        /// <summary>
        /// Gets the description attribute value from a field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The value of the <see cref="DescriptionAttribute"/>.</returns>
        /// <remarks></remarks>
        public static String GetDescriptionAttributeValueFromField(Object field)
        {
            var objectField = field.GetType().GetField(field.ToString());
            var descriptionAttribute = objectField.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

            return (descriptionAttribute != null) ? descriptionAttribute.Description : String.Empty;
        }

        /// <summary>
        /// Gets the enum value from the specified description attribute value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="description">The description.</param>
        /// <returns>The <typeparamref name="TEnum"/> value.</returns>
        /// <remarks></remarks>
        public static TEnum GetEnumValueFromDescriptionAttributeValue<TEnum>(String description)
        {
            var field =
                typeof(TEnum).GetFields()
                             .ToList()
                             .FirstOrDefault(fi => 
                                 fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                   .Cast<DescriptionAttribute>()
                                   .Any(a => String.Compare(description, a.Description, StringComparison.OrdinalIgnoreCase) == 0));

            if (field == null)
            {
                throw new ArgumentOutOfRangeException("description", "Invalid argument. The enum does not contain a description attribute with the value supplied.");
            }

            return (TEnum)field.GetValue(null);
        }
    }
}