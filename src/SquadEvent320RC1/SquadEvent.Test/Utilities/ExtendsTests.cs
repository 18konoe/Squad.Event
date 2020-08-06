using System;
using System.Collections.Generic;
using System.Security.Claims;
using Moq;
using NUnit.Framework;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Test.Utilities
{
    [TestFixture]
    public class ExtendsTests
    {
        [Test]
        public void GetDiscordUserId()
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, "test"));
            claims.GetDiscordUserId().Is("test");
        }

        [Test]
        public void GetDiscordEmail()
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, "test"));
            claims.GetDiscordEmail().Is("test");
        }

        [Test]
        public void ElementalEqualsTest1()
        {
            IEnumerable<string> nullCollection = null;
            IEnumerable<string> nonNullCollection = new List<string>(){ string.Empty };
            nonNullCollection.ElementalEquals(nullCollection).IsFalse();
            nullCollection.ElementalEquals(nonNullCollection).IsFalse();
            nullCollection.ElementalEquals(nullCollection).IsTrue();
        }

        [Test]
        public void ElementalEqualsTest2()
        {
            DateTimeOffset testDateTime = DateTimeOffset.Now;
            IEnumerable<string> target1 = new List<string>();
            IEnumerable<string> target2 = new List<string>();

            target1.ElementalEquals(target2).IsTrue();
            target2.ElementalEquals(target1).IsTrue();
        }

        [Test]
        public void ElementalEqualsTest3()
        {
            DateTimeOffset testDateTime = DateTimeOffset.Now;
            IEnumerable<string> target1 = new List<string>()
            {
                "test1"
            };
            IEnumerable<string> target2 = new List<string>();

            target1.ElementalEquals(target2).IsFalse();
            target2.ElementalEquals(target1).IsFalse();
        }

        [Test]
        public void ElementalEqualsTest4()
        {
            DateTimeOffset testDateTime = DateTimeOffset.Now;
            IEnumerable<string> target1 = new List<string>()
            {
                "test1"
            };
            IEnumerable<string> target2 = new List<string>()
            {
                "test2"
            };

            target1.ElementalEquals(target2).IsFalse();
            target2.ElementalEquals(target1).IsFalse();
        }

        [Test]
        public void ElementalEqualsTest5()
        {
            DateTimeOffset testDateTime = DateTimeOffset.Now;
            IEnumerable<string> target1 = new List<string>()
            {
                "test1"
            };
            IEnumerable<string> target2 = new List<string>()
            {
                "test1"
            };

            target1.ElementalEquals(target2).IsTrue();
            target2.ElementalEquals(target1).IsTrue();
        }

        [Test]
        public void ElementalEqualsTest6()
        {
            DateTimeOffset testDateTime = DateTimeOffset.Now;
            IEnumerable<string> target1 = new List<string>()
            {
                "test1",
                "test2"
            };
            IEnumerable<string> target2 = new List<string>()
            {
                "test2",
                "test1"
            };

            target1.ElementalEquals(target2).IsTrue();
            target2.ElementalEquals(target1).IsTrue();
        }

        [Test]
        public void ElementalEqualsTest7()
        {
            DateTimeOffset testDateTime = DateTimeOffset.Now;
            IEnumerable<string> target1 = new List<string>()
            {
                "test3",
                "test1",
                "test2"
            };
            IEnumerable<string> target2 = new List<string>()
            {
                "test2",
                "test1"
            };

            target1.ElementalEquals(target2).IsFalse();
            target2.ElementalEquals(target1).IsFalse();
        }

        [Test]
        public void ValueContainsTest1()
        {
            IEnumerable<ITestElementClass> target = null;
            Assert.Catch<ArgumentException>(() => target.ValueContains(null));
        }

        [Test]
        public void ValueContainsTest2()
        {
            IEnumerable<ITestElementClass> target = new List<ITestElementClass>();
            target.ValueContains(null).IsFalse();
        }

        [Test]
        public void ValueContainsTest3()
        {
            var mock1 = new Mock<ITestElementClass>();
            var mock2 = new Mock<ITestElementClass>();
            var mock3 = new Mock<ITestElementClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            DateTimeOffset testDateTime = DateTimeOffset.Now;
            IEnumerable<ITestElementClass> target = new List<ITestElementClass>()
            {
                mock1.Object,
                mock2.Object
            };
            target.ValueContains(mock3.Object).IsTrue();
        }

        [Test]
        public void ValueEqualsDictionaryTest01()
        {
            IDictionary<string, ITestValueClass> target = null;
            IDictionary<string, ITestValueClass> other = null;

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsDictionaryTest02()
        {
            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>();
            IDictionary<string, ITestValueClass> other = null;

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsDictionaryTest03()
        {
            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", null},
                {"key2", null},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key1", null},
            };

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsDictionaryTest04()
        {
            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", null},
                {"key3", null},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key1", null},
                {"key2", null},
            };

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsDictionaryTest05()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock2 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();
            var mock4 = new Mock<ITestValueClass>();

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock1.Object},
                {"key2", mock2.Object},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock3.Object},
                {"key2", mock4.Object},
            };

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsDictionaryTest06()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock2 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();
            var mock4 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock2.Setup(obj => obj.ValueEquals(mock4.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);
            mock4.Setup(obj => obj.ValueEquals(mock2.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock1.Object},
                {"key2", mock2.Object},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock3.Object},
                {"key2", mock4.Object},
            };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsDictionaryTest07()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock2 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();
            var mock4 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock2.Setup(obj => obj.ValueEquals(mock4.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);
            mock4.Setup(obj => obj.ValueEquals(mock2.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock1.Object},
                {"key2", mock2.Object},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key2", mock4.Object},
                {"key1", mock3.Object},
            };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsDictionaryTest08()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key2", null},
                {"key1", mock1.Object},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key2", null},
                {"key1", mock3.Object},
            };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsDictionaryTest09()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock1.Object},
                {"key2", null},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key2", null},
                {"key1", mock3.Object},
            };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsDictionaryTest10()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock1.Object},
                {"key2", null},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock3.Object},
                {"key2", null},
            };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsDictionaryTest11()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key2", null},
                {"key1", mock1.Object},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock3.Object},
                {"key2", null},
            };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsDictionaryTest12()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key2", null},
                {"key1", mock1.Object},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key2", mock3.Object},
                {"key1", null},
            };

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsDictionaryTest13()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key2", null},
                {"key1", mock1.Object},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key1", null},
                {"key2", mock3.Object},
            };

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsDictionaryTest14()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock1.Object},
                {"key2", null},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key2", mock3.Object},
                {"key1", null},
            };

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsDictionaryTest15()
        {
            var mock1 = new Mock<ITestValueClass>();
            var mock3 = new Mock<ITestValueClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);

            IDictionary<string, ITestValueClass> target = new Dictionary<string, ITestValueClass>()
            {
                {"key1", mock1.Object},
                {"key2", null},
            };
            IDictionary<string, ITestValueClass> other = new Dictionary<string, ITestValueClass>()
            {
                {"key1", null},
                {"key2", mock3.Object},
            };

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsEnumerableTest1()
        {
            IEnumerable<ITestElementClass> target = null;
            IEnumerable<ITestElementClass> other = null;

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsEnumerableTest2()
        {
            IEnumerable<ITestElementClass> target = new List<ITestElementClass>();
            IEnumerable<ITestElementClass> other = null;

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsEnumerableTest3()
        {
            IEnumerable<ITestElementClass> target = new List<ITestElementClass>() {null, null};
            IEnumerable<ITestElementClass> other = new List<ITestElementClass>() {null};

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsEnumerableTest4()
        {
            IEnumerable<ITestElementClass> target = new List<ITestElementClass>() { null, null };
            IEnumerable<ITestElementClass> other = new List<ITestElementClass>() { null, null };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsEnumerableTest5()
        {
            var mock1 = new Mock<ITestElementClass>();
            var mock2 = new Mock<ITestElementClass>();
            var mock3 = new Mock<ITestElementClass>();
            var mock4 = new Mock<ITestElementClass>();

            IEnumerable<ITestElementClass> target = new List<ITestElementClass>()
            {
                mock1.Object,
                mock2.Object
            };
            IEnumerable<ITestElementClass> other = new List<ITestElementClass>()
            {
                mock3.Object,
                mock4.Object
            };

            target.ValueEquals(other).IsFalse();
            other.ValueEquals(target).IsFalse();
        }

        [Test]
        public void ValueEqualsEnumerableTest6()
        {
            var mock1 = new Mock<ITestElementClass>();
            var mock2 = new Mock<ITestElementClass>();
            var mock3 = new Mock<ITestElementClass>();
            var mock4 = new Mock<ITestElementClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock2.Setup(obj => obj.ValueEquals(mock4.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);
            mock4.Setup(obj => obj.ValueEquals(mock2.Object)).Returns(true);

            IEnumerable<ITestElementClass> target = new List<ITestElementClass>()
            {
                mock1.Object,
                mock2.Object
            };
            IEnumerable<ITestElementClass> other = new List<ITestElementClass>()
            {
                mock3.Object,
                mock4.Object
            };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsEnumerableTest7()
        {
            var mock1 = new Mock<ITestElementClass>();
            var mock2 = new Mock<ITestElementClass>();
            var mock3 = new Mock<ITestElementClass>();
            var mock4 = new Mock<ITestElementClass>();

            mock1.Setup(obj => obj.ValueEquals(mock3.Object)).Returns(true);
            mock2.Setup(obj => obj.ValueEquals(mock4.Object)).Returns(true);
            mock3.Setup(obj => obj.ValueEquals(mock1.Object)).Returns(true);
            mock4.Setup(obj => obj.ValueEquals(mock2.Object)).Returns(true);

            IEnumerable<ITestElementClass> target = new List<ITestElementClass>()
            {
                mock2.Object,
                mock1.Object
            };
            IEnumerable<ITestElementClass> other = new List<ITestElementClass>()
            {
                mock3.Object,
                mock4.Object
            };

            target.ValueEquals(other).IsTrue();
            other.ValueEquals(target).IsTrue();
        }


        [Test]
        public void ValueEqualsIntegrateTest()
        {
            DateTimeOffset testDateTime = DateTimeOffset.Now;
            ITestSurfaceClass target1 = new TestSurfaceClass();
            target1.Name = "TestSurfaceName";
            target1.Date = testDateTime;
            target1.Values.Add("TestKey1",
                new TestValueClass()
                {
                    Name = "TestKey1Name",
                    Elements = new List<ITestElementClass>() {new TestElementClass(testDateTime + TimeSpan.FromDays(1), TestEnum.Test1)}
                });
            target1.Values.Add("TestKey2",
                new TestValueClass()
                {
                    Name = "TestKey2Name",
                    Elements = new List<ITestElementClass>() { new TestElementClass(testDateTime + TimeSpan.FromDays(1), TestEnum.Test2) }
                });

            ITestSurfaceClass target2 = new TestSurfaceClass();
            target2.Name = "TestSurfaceName";
            target2.Date = testDateTime;
            target2.Values.Add("TestKey2",
                new TestValueClass()
                {
                    Name = "TestKey2Name",
                    Elements = new List<ITestElementClass>() { new TestElementClass(testDateTime + TimeSpan.FromDays(1), TestEnum.Test2) }
                });
            target2.Values.Add("TestKey1",
                new TestValueClass()
                {
                    Name = "TestKey1Name",
                    Elements = new List<ITestElementClass>() { new TestElementClass(testDateTime + TimeSpan.FromDays(1), TestEnum.Test1) }
                });

            target1.ValueEquals(target2).IsTrue();
        }
    }

    
}