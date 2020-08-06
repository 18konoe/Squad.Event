using NUnit.Framework;
using SquadEvent.Shared.Parameters;

namespace SquadEvent.Test.Parameters
{
    [TestFixture]
    public class EventIdResponseTests
    {
        [Test]
        public void InitializeTest1()
        {
            var target = new EventIdResponse();
            target.IsNotNull();
            target.Id.IsNull();
        }

        [Test]
        public void InitializeTest2()
        {
            var target = new EventIdResponse("test");
            target.IsNotNull();
            target.Id.Is("test");
        }
    }
}