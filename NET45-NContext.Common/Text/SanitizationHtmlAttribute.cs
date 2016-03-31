namespace NContext.Common.Text
{
    using System;

    /// <summary>
    /// Marker interface that instructs <see cref="ObjectGraphSanitizer"/> to sanitize the target member as html.
    /// The graph sanitizer will use the <see cref="ISanitizeText.SanitizeHtml"/> method instead of <see cref="ISanitizeText.SanitizeHtmlFragment"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SanitizationHtmlAttribute : Attribute
    {
    }
}