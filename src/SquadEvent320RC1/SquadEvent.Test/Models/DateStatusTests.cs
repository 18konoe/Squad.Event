using System;
using NUnit.Framework;
using SquadEvent.Shared.Models;

namespace SquadEvent.Test.Models
{
    [TestFixture]
    public class DateStatusTests
    {
        [Test]
        public void InitializeTest()
        {
            DateTimeOffset testTime = DateTimeOffset.Now;
            var target = new DateStatus(testTime, AttendanceStatus.Absent);
            
            target.IsNotNull();
            target.Date.Is(testTime);
            target.Status = AttendanceStatus.Absent;
        }

        [Test]
        public void ValueEqualsTest1()
        {
            DateTimeOffset testTime = DateTimeOffset.Now;
            var target = new DateStatus(testTime, AttendanceStatus.Absent);
            
            target.ValueEquals(null).IsFalse();
        }

        [Test]
        public void ValueEqualsTest2()
        {
            DateTimeOffset testTime = DateTimeOffset.Now;
            var target = new DateStatus(testTime, AttendanceStatus.Absent);

            target.ValueEquals(target).IsTrue();
        }

        [Test]
        public void ValueEqualsTest3()
        {
            DateTimeOffset testTime = DateTimeOffset.Now;
            var target1 = new DateStatus(testTime, AttendanceStatus.Absent);
            var target2 = new DateStatus(testTime + TimeSpan.FromDays(1), AttendanceStatus.Absent);

            target1.ValueEquals(target2).IsFalse();
        }

        [Test]
        public void ValueEqualsTest4()
        {
            DateTimeOffset testTime = DateTimeOffset.Now;
            var target1 = new DateStatus(testTime, AttendanceStatus.Absent);
            var target2 = new DateStatus(testTime, AttendanceStatus.Hope);

            target1.ValueEquals(target2).IsFalse();
        }

        [Test]
        public void ValueEqualsTest5()
        {
            DateTimeOffset testTime = DateTimeOffset.Now;
            var target1 = new DateStatus(testTime, AttendanceStatus.Hope);
            var target2 = new DateStatus(testTime, AttendanceStatus.Hope);

            target1.ValueEquals(target2).IsTrue();
        }
    }
}