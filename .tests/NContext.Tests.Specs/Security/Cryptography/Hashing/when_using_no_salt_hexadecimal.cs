namespace NContext.Tests.Specs.Security.Cryptography
{
    using System;

    using Machine.Specifications;

    public class when_using_no_salt_hexadecimal : when_hashing_with_HashProvider
    {
        Establish context = () =>
        {
            _TargetPlainText = "NContext";
        };
        
        Because of = () => _SourceHash = HashProvider.CreateHash(_TargetPlainText, 0);

        It should_equal_the_hex_value = () => HashProvider.CompareHash(_TargetPlainText, _SourceHash, 0).ShouldEqual(true);

        private static String _TargetPlainText;

        private static String _SourceHash;
    }
}