using NUnit.Framework;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Test.Utilities
{
    [TestFixture]
    public class Sha256HashStringTest
    {
        private const string SALT = "f977c2863591a9691a18000af370c519";
        private const string SERIAL = "YHG010DQ";
        private const string RESULT_EXPECTED_STRING = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";

        private static readonly byte[] RESULT_EXPECTED_BYTES = new byte[]
        {
            0xE1, 0xFC, 0x30, 0x9C, 0x5B, 0x67, 0xFC, 0x16, 0xF4, 0xCA, 0x81, 0x69, 0x61, 0x49, 0x80, 0x61, 0xAB, 0xB5,
            0xB8, 0x79, 0xB9, 0x8A, 0xA4, 0xF0, 0x31, 0x31, 0xEA, 0x62, 0x77, 0xFC, 0xA9, 0xEA
        };

        [Test]
        public void InitializeTest()
        {
            var target = new Sha256HashString(SALT, SERIAL);
            target.IsNotNull();
            target.Salt.Is(string.Join("", SALT, SERIAL));
        }

        [Test]
        public void ToHashTest()
        {
            var target = new Sha256HashString(SALT, SERIAL);
            target.IsNotNull();
            target.ToHash().Is(RESULT_EXPECTED_BYTES);
        }

        [Test]
        public void ToStringTest()
        {
            var target = new Sha256HashString(SALT, SERIAL);
            target.IsNotNull();
            target.ToString().Is(RESULT_EXPECTED_STRING);
        }
    }
}