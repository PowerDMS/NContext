namespace NContext.Tests.Specs.Text
{
    using System;

    using NContext.Common.Text;

    public class DummySite
    {
        public String Description { get; set; }

        [SanitizationHtml]
        public String Message { get; set; }

        [SanitizationIgnore]
        public String Name { get; set; }
    }
}