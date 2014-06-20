namespace NContext.Tests.Specs.Security.Cryptography.SymmetricEncryption
{
    using System.Security.Cryptography;

    using Machine.Specifications;

    using NContext.Security.Cryptography;

    public abstract class when_using_SymmetricEncryptionProvider<TSymmetricAlgorithm> where TSymmetricAlgorithm : SymmetricAlgorithm
    {
        Establish ctx = () =>
        {
            Provider = new SymmetricEncryptionProvider(typeof (TSymmetricAlgorithm));
        };

        protected static SymmetricEncryptionProvider Provider { get; private set; }
    }
}