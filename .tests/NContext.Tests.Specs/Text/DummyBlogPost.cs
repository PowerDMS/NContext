namespace NContext.Tests.Specs.Text
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class DummyBlogPost
    {
        private ReadOnlyCollection<TagDto> _Tags;

        public Int32 BlogId { get; set; }

        public DummyBlogAuthor Author { get; set; }

        public String Title { get; set; }

        public String Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? PublishedOn { get; set; }

        public IEnumerable<String> Tags { get; set; }

        public ICollection<TagDto> ComplexReadOnlyCollection
        {
            get { return _Tags; }
            set { _Tags = new ReadOnlyCollection<TagDto>(value == null ? new List<TagDto>() : value.ToList()); }
        }

        public IDictionary<Int32, String> Comments { get; set; }

        public DummyBlogLink[] Links { get; set; }

        public List<String> References { get; set; }
    }
}