namespace SquadEvent.Shared.Utilities
{
    public class ByteToStringConverter : ICustomConverter<byte[], string>
    {
        private readonly uint[] _lookup32;
        public ByteToStringConverter()
        {
            _lookup32 = CreateLookup32();
        }

        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = $"{i:X2}";
                result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
            }
            return result;
        }

        public string Convert(byte[] bytes)
        {
            if (bytes is null)
            {
                return string.Empty;
            }

            var result = new char[bytes.Length * 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                var val = _lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }

            return new string(result);
        }
    }
}