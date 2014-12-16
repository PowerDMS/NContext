namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;

    public class DummyBlogPost
    {
        public Int32 BlogId { get; set; }

        public DummyBlogAuthor Author { get; set; }

        public String Title { get; set; }

        public String Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? PublishedOn { get; set; }

        public IEnumerable<String> Tags { get; set; }

        public IDictionary<Int32, String> Comments { get; set; }

        public DummyBlogLink[] Links { get; set; }

        public List<String> References { get; set; }
    }
}