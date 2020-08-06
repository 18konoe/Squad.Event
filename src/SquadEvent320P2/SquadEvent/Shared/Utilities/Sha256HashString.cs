using System.Security.Cryptography;
using System.Text;

namespace SquadEvent.Shared.Utilities
{
    public class Sha256HashString : IHashString
    {
        public ICustomConverter<byte[], string> Converter { get; } = new ByteToStringConverter();
        public string Salt { get; }
        public Sha256HashString(params string[] salts)
        {
            Salt = string.Join("", salts);
        }

        public override string ToString()
        {
            var hashed = ToHash();
            return hashed == null ? string.Empty : Converter.Convert(hashed);
        }

        public byte[] ToHash()
        {
            using (SHA256 sha256 = new SHA256CryptoServiceProvider())
            {
                byte[] input = Encoding.UTF8.GetBytes(Salt);
                return sha256.ComputeHash(input);
            }
        }
    }
}