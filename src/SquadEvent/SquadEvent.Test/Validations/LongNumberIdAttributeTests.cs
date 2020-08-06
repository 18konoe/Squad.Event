using NUnit.Framework;
using SquadEvent.Shared.Validations;

namespace SquadEvent.Test.Validations
{
    [TestFixture]
    public class LongNumberIdAttributeTests
    {
        [TestCase(null, ExpectedResult = false)]
        [TestCase(0x1234567890abcdef, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEFgG", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEF", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEF00", ExpectedResult = false)]
        [TestCase("0", ExpectedResult = true)]
        [TestCase("98765432109876543210", ExpectedResult = true)]
        [TestCase("987654321098765432100", ExpectedResult = false)]
        public bool ValidationResultTest(object obj)
        {
            var target = new LongNumberIdAttribute();
            return target.IsValid(obj);
        }
    }
}