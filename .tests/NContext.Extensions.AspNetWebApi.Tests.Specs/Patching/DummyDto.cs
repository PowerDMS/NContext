namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Patching
{
    using System;

    internal class DummyDto
    {
        public Int32 Id { get; set; }

        public String FirstName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DisabledOn { get; set; }

        public Boolean IsAdmin { get; set; }

        public Int32? SpaceAmount { get; set; }

        public Int32 SpaceUsed { get; set; }
    }
}