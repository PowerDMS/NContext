namespace NContext.Text
{
    using System;

    /// <summary>
    /// Defines an abstraction to santize user-input strings.
    /// </summary>
    public interface ISanitizeText
    {
        /// <summary>
        /// Sanitizes the specified text.
        /// </summary>
        /// <param name="textToSanitize">The text to sanitize.</param>
        /// <returns>String.</returns>
        String SanitizeHtml(String textToSanitize);

        /// <summary>
        /// Sanitizes the specified text.
        /// </summary>
        /// <param name="textToSanitize">The text to sanitize.</param>
        /// <returns>String.</returns>
        String SanitizeHtmlFragment(String textToSanitize);
    }
}