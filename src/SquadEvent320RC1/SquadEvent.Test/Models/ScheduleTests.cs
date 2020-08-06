using System;
using NUnit.Framework;
using SquadEvent.Shared.Models;

namespace SquadEvent.Test.Models
{
    [TestFixture]
    public class ScheduleTests
    {
        [Test]
        public void ScheduleEqualsFailedTest1()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));

            bool result = schedule1.ValueEquals(null);
            result.IsFalse();
        }

        [Test]
        public void ScheduleEqualsSucceededTest1()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));

            bool result = schedule1.ValueEquals(schedule1);
            result.IsTrue();
        }

        [Test]
        public void ScheduleEqualsFailedTest2()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1IdDiff";

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsFalse();
        }

        [Test]
        public void ScheduleEqualsFailedTest3()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1Id";
            schedule1alt.Name = "Key1NameDiff";

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsFalse();
        }

        [Test]
        public void ScheduleEqualsFailedTest4()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1Id";
            schedule1alt.Name = "Key1Name";
            schedule1alt.Comment = "Key1CommentDiff";

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsFalse();
        }

        [Test]
        public void ScheduleEqualsFailedTest5()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1Id";
            schedule1alt.Name = "Key1Name";
            schedule1alt.Comment = "Key1Comment";
            schedule1alt.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Difficult));

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsFalse();
        }

        [Test]
        public void ScheduleEqualsFailedTest6()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1Id";
            schedule1alt.Name = "Key1Name";
            schedule1alt.Comment = "Key1Comment";
            schedule1alt.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Absent));

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsFalse();
        }

        [Test]
        public void ScheduleEqualsFailedTest7()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1Id";
            schedule1alt.Name = "Key1Name";
            schedule1alt.Comment = "Key1Comment";
            schedule1alt.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Hope));

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsFalse();
        }

        [Test]
        public void ScheduleEqualsFailedTest8()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1Id";
            schedule1alt.Name = "Key1Name";
            schedule1alt.Comment = "Key1Comment";
            schedule1alt.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            schedule1alt.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsFalse();
        }

        [Test]
        public void ScheduleEqualsSucceededTest2()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            schedule1.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1Id";
            schedule1alt.Name = "Key1Name";
            schedule1alt.Comment = "Key1Comment";
            schedule1alt.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            schedule1alt.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsTrue();
        }

        [Test]
        public void ScheduleEqualsSucceededTest3()
        {
            var schedule2 = new Schedule();
            schedule2.UserId = "Key2Id";
            schedule2.Name = "Key2Name";
            schedule2.Comment = "Key2Comment";
            schedule2.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));
            schedule2.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));

            var schedule2alt = new Schedule();
            schedule2alt.UserId = "Key2Id";
            schedule2alt.Name = "Key2Name";
            schedule2alt.Comment = "Key2Comment";
            schedule2alt.DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));
            schedule2alt.DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));

            bool result = schedule2.ValueEquals(schedule2alt);
            result.IsTrue();
        }

        [Test]
        public void ScheduleEqualsSucceededTest4()
        {
            var schedule1 = new Schedule();
            schedule1.UserId = "Key1Id";
            schedule1.Name = "Key1Name";
            schedule1.Comment = "Key1Comment";

            var schedule1alt = new Schedule();
            schedule1alt.UserId = "Key1Id";
            schedule1alt.Name = "Key1Name";
            schedule1alt.Comment = "Key1Comment";

            bool result = schedule1.ValueEquals(schedule1alt);
            result.IsTrue();
        }
    }
}