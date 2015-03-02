namespace NContext.Tests.Specs.Text
{
    using System;

    using Machine.Specifications;

    using NContext.Text;

    public abstract class when_sanitizing_objects_with_ObjectGraphSanitizer
    {
        Establish context = () =>
            {
                _MaxDegreeOfParallelism = 1;
                _Sanitizer = new Lazy<ObjectGraphSanitizer>(() => new ObjectGraphSanitizer(TextSanitizer, MaxDegreeOfParallelism));
            };

        protected static ISanitizeText TextSanitizer { get; set; }

        protected static Int32 MaxDegreeOfParallelism
        {
            get { return _MaxDegreeOfParallelism; }
            set { _MaxDegreeOfParallelism = value; }
        }

        protected static void Sanitize(Object o)
        {
            _Sanitizer.Value.Sanitize(o);
        }

        static Lazy<ObjectGraphSanitizer> _Sanitizer;

        static Int32 _MaxDegreeOfParallelism;
    }
}