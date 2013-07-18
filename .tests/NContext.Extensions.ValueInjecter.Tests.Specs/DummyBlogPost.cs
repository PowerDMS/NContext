namespace NContext.Extensions.ValueInjecter.Tests.Specs
{
    using System;

    public class DummyBlogPost
    {
        public Int32 Id { get; set; }

        public Guid BlogId { get; set; }

        public String Title { get; set; }

        public String Summary { get; set; }

        public String Post { get; set; }

        public DateTime? PublishedOn { get; set; }

        public DummyBlogPostStatus Status { get; set; }
    }
}