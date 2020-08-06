using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using SquadEvent.Shared.Validations;

namespace SquadEvent.Test.Validations
{
    [TestFixture]
    public class BsonIdHexStringsAttributeTests
    {
        [TestCase(null, ExpectedResult = false)]
        [TestCase(0x1234567890abcdef, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEFgG", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEF", ExpectedResult = false)]
        [TestCase("0123456789abcdefABCDEF00", ExpectedResult = true)]
        public bool ValidationResultTest(object obj)
        {
            var target = new BsonIdHexStringsAttribute();
            return target.IsValid(obj);
        }
    }
}