namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class DummyBlogAuthor
    {
        public Guid AuthorId { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public IEnumerable<DummyBlogPost> BlogPosts { get; set; }

        public Collection<String> Websites { get; set; }
    }
}