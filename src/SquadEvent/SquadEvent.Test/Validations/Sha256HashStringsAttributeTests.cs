using NUnit.Framework;
using SquadEvent.Shared.Validations;

namespace SquadEvent.Test.Validations
{
    [TestFixture]
    public class Sha256HashStringsAttributeTests
    {
        [TestCase(null, ExpectedResult = false)]
        [TestCase(0x1234567890abcdef, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEFgG", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEF", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEF00", ExpectedResult = false)]
        [TestCase("0", ExpectedResult = false)]
        [TestCase("98765432109876543210", ExpectedResult = false)]
        [TestCase("987654321098765432100", ExpectedResult = false)]
        [TestCase("1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef", ExpectedResult = true)]
        [TestCase("1234567890ABCDEF1234567890ABCDEF1234567890ABCDEF1234567890ABCDEF", ExpectedResult = true)]
        public bool ValidationResultTest(object obj)
        {
            var target = new Sha256HashStringsAttribute();
            return target.IsValid(obj);
        }
    }
}