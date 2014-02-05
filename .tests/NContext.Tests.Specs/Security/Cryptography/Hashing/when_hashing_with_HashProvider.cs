namespace NContext.Tests.Specs.Security.Cryptography
{
    using System.Security.Cryptography;

    using Machine.Specifications;

    using NContext.Security.Cryptography;

    public abstract class when_hashing_with_HashProvider
    {
        Establish context = () =>
        {
            _HashProvider = new HashProvider(typeof (SHA1CryptoServiceProvider));
        };

        protected static IProvideHashing HashProvider
        {
            get { return _HashProvider; }
        }

        private static IProvideHashing _HashProvider;
    }
}