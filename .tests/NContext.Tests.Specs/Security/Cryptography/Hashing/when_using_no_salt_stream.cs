namespace NContext.Tests.Specs.Security.Cryptography
{
    using System;
    using System.IO;
    using System.Text;

    using Machine.Specifications;

    using NContext.Extensions;

    public class when_using_no_salt_stream : when_hashing_with_HashProvider
    {
        Establish context = () => _PlainText = new MemoryStream("NContext".ToBytes<UTF8Encoding>());

        Because of = () => _SourceHash = HashProvider.CreateHash(_PlainText, 0);

        It should_be_of_fixed_length = () => _SourceHash.Length.ShouldEqual(20);

        It should_equal_the_target_Byte_array = () =>
        {
            _PlainText = new MemoryStream("NContext".ToBytes<UTF8Encoding>()); // Reset stream because CreateHash(Stream) will close it.
            HashProvider.CompareHash(_PlainText, _SourceHash, 0).ShouldBeTrue();
        };

        Cleanup _ = () => _PlainText.Dispose();
        
        private static Byte[] _SourceHash;

        private static Stream _PlainText;
    }
}