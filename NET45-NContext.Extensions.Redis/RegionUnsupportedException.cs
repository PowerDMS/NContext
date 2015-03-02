namespace NContext.Extensions.Redis
{
    using System;

    public class RegionUnsupportedException : Exception
    {
        public RegionUnsupportedException() : base("Redis does not support cache regions.")
        {
        }
    }
}