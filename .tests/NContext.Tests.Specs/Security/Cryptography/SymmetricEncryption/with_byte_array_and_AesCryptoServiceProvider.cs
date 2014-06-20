namespace NContext.Tests.Specs.Security.Cryptography.SymmetricEncryption
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Machine.Specifications;

    public class with_byte_array_and_AesCryptoServiceProvider : when_using_SymmetricEncryptionProvider<AesCryptoServiceProvider>
    {
        Because i_encrypt_the_plain_text = () => _CipherText = Provider.Encrypt(_SymmetricKey, _PlainText);

        It should_decrypt_back_to_plain_text = () => Provider.Decrypt(_SymmetricKey, _CipherText).ShouldEqual(_PlainText);

        static readonly Byte[] _PlainText = Encoding.UTF8.GetBytes("ncontext");

        static Byte[] _CipherText;

        static readonly Byte[] _SymmetricKey = {
            0xe5, 0xd4, 0xe5, 0x9a, 0x84, 0xae, 0xa1, 0xa0, 0xa9, 0xd0, 0x70, 0xd9,
            0x30, 0x32, 0xbd, 0x8e, 0xdf, 0x70, 0x29, 0xd3, 0x9c, 0x83, 0x21, 0xc3,
            0x7e, 0x7d, 0x10, 0xc5, 0x55, 0x24, 0x46, 0x12
        };
    }
}