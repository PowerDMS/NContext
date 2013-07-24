namespace NContext.Common.Tests.Specs
{
    using System;

    public class DummyData2
    {
        readonly int _Id;

        public DummyData2(Int32 id)
        {
            _Id = id;
        }

        public int Id
        {
            get { return _Id; }
        }
    }
}