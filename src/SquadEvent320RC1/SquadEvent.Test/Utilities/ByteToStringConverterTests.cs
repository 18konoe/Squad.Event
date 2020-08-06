using NUnit.Framework;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Test.Utilities
{
    [TestFixture]
    public class ByteToStringConverterTests
    {
        private const string RESULT_EXPECTED_STRING_1 = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";

        private static readonly byte[] RESULT_EXPECTED_BYTES = new byte[]
        {
            0xE1, 0xFC, 0x30, 0x9C, 0x5B, 0x67, 0xFC, 0x16, 0xF4, 0xCA, 0x81, 0x69, 0x61, 0x49, 0x80, 0x61, 0xAB, 0xB5,
            0xB8, 0x79, 0xB9, 0x8A, 0xA4, 0xF0, 0x31, 0x31, 0xEA, 0x62, 0x77, 0xFC, 0xA9, 0xEA
        };
        
        [Test]
        public void ConvertTest1()
        {
            var target = new ByteToStringConverter();
            target.Convert(null).Is(string.Empty);
        }

        [Test]
        public void ConvertTest2()
        {
            var target = new ByteToStringConverter();
            target.Convert(RESULT_EXPECTED_BYTES).Is(RESULT_EXPECTED_STRING_1);
        }
    }
}