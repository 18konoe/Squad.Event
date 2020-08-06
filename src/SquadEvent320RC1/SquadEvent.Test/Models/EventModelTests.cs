using System;
using System.Collections.Generic;
using NUnit.Framework;
using SquadEvent.Shared.Models;

namespace SquadEvent.Test.Models
{
    [TestFixture]
    public class EventModelTests
    {
        [Test]
        public void EventEqualsFailedTest01()
        {
            var event1 = new EventModel();
            DateTime lastUpdate = DateTime.Now;
            event1.LastUpdated = lastUpdate;
            event1.Schedules.Add("key1", new Schedule());
            event1.Schedules["key1"].UserId = "Key1Id";
            event1.Schedules["key1"].Name = "Key1Name";
            event1.Schedules["key1"].Comment = "Key1Comment";
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));
            event1.Schedules.Add("key2", new Schedule());
            event1.Schedules["key2"].UserId = "Key2Id";
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));
            event1.Editors.Add("User2");
            event1.Editors.Add("User1");
            event1.Members.Add("Member1");
            event1.Members.Add("Member2");
            event1.Dates.Add(lastUpdate);
            event1.Dates.Add(lastUpdate + TimeSpan.FromDays(1));
            
            bool result = event1.ValueEquals(null);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsSucceededTest1()
        {
            var event1 = new EventModel();
            DateTime lastUpdate = DateTime.Now;
            event1.LastUpdated = lastUpdate;
            event1.Schedules.Add("key1", new Schedule());
            event1.Schedules["key1"].UserId = "Key1Id";
            event1.Schedules["key1"].Name = "Key1Name";
            event1.Schedules["key1"].Comment = "Key1Comment";
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));
            event1.Schedules.Add("key2", new Schedule());
            event1.Schedules["key2"].UserId = "Key2Id";
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));
            event1.Editors.Add("User2");
            event1.Editors.Add("User1");
            event1.Members.Add("Member1");
            event1.Members.Add("Member2");
            event1.Dates.Add(lastUpdate);
            event1.Dates.Add(lastUpdate + TimeSpan.FromDays(1));

            bool result = event1.ValueEquals(event1);
            result.IsTrue();
        }

        [Test]
        public void EventEqualsFailedTest03()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;

            var event2 = new EventModel();
            event2.Id = "TestIdDiff";
            event2.LastUpdated = lastUpdate + TimeSpan.FromHours(1);

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest04()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginatorDiff";

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest05()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestNameDiff";

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest06()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescriptionDiff";

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest07()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Fixed;

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest08()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.Editor;

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest09()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannelDiff";

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest10()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuildDiff";

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest11()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = null;

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest12()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = null;

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest13()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate;

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest14()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event1.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event2.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EB";

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest15()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event1.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event1.Editors = new List<string>() { "TestEditor1", "TestEditor2" };

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event2.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event2.Editors = new List<string>() { "TestEditor1" };

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest16()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event1.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event1.Editors = new List<string>() { "TestEditor1"};

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event2.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event2.Editors = new List<string>() { "TestEditor1", "TestEditor2" };

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest17()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event1.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event1.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event1.Members = new List<string>() { "TestMembers1" };

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event2.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event2.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event2.Members = new List<string>() { "TestMembers1", "TestMembers2" };

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest18()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event1.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event1.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event1.Members = new List<string>() { "TestMembers1", "TestMembers2" };
            event1.Dates.Add(lastUpdate);
            event1.Dates.Add(lastUpdate + TimeSpan.FromDays(1));

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event2.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event2.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event2.Members = new List<string>() { "TestMembers1", "TestMembers2" };
            event2.Dates.Add(lastUpdate);
            event2.Dates.Add(lastUpdate + TimeSpan.FromDays(2));

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsFailedTest19()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event1.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event1.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event1.Members = new List<string>() { "TestMembers1", "TestMembers2" };
            event1.Dates.Add(lastUpdate);
            event1.Dates.Add(lastUpdate + TimeSpan.FromDays(1));
            event1.Schedules.Add("key1", new Schedule());
            event1.Schedules["key1"].UserId = "Key1Id";
            event1.Schedules["key1"].Name = "Key1Name";
            event1.Schedules["key1"].Comment = "Key1Comment";
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));
            event1.Schedules.Add("key2", new Schedule());
            event1.Schedules["key2"].UserId = "Key2Id";
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event2.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event2.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event2.Members = new List<string>() { "TestMembers1", "TestMembers2" };
            event2.Dates.Add(lastUpdate);
            event2.Dates.Add(lastUpdate + TimeSpan.FromDays(1));
            event2.Schedules.Add("key1", new Schedule());
            event2.Schedules["key1"].UserId = "Key1Id";
            event2.Schedules["key1"].Name = "Key1Name";
            event2.Schedules["key1"].Comment = "Key1Comment";
            event2.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            event2.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));

            bool result = event1.ValueEquals(event2);
            result.IsFalse();
        }

        [Test]
        public void EventEqualsSucceededTest2()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event1.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event1.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event1.Members = new List<string>() { "TestMembers1", "TestMembers2" };
            event1.Dates.Add(lastUpdate);
            event1.Dates.Add(lastUpdate + TimeSpan.FromDays(1));
            event1.Schedules.Add("key1", new Schedule());
            event1.Schedules["key1"].UserId = "Key1Id";
            event1.Schedules["key1"].Name = "Key1Name";
            event1.Schedules["key1"].Comment = "Key1Comment";
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));
            event1.Schedules.Add("key2", new Schedule());
            event1.Schedules["key2"].UserId = "Key2Id";
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event2.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event2.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event2.Members = new List<string>() { "TestMembers1", "TestMembers2" };
            event2.Dates.Add(lastUpdate);
            event2.Dates.Add(lastUpdate + TimeSpan.FromDays(1));
            event2.Schedules.Add("key1", new Schedule());
            event2.Schedules["key1"].UserId = "Key1Id";
            event2.Schedules["key1"].Name = "Key1Name";
            event2.Schedules["key1"].Comment = "Key1Comment";
            event2.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            event2.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));
            event2.Schedules.Add("key2", new Schedule());
            event2.Schedules["key2"].UserId = "Key2Id";
            event2.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));
            event2.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));

            bool result = event1.ValueEquals(event2);
            result.IsTrue();
        }

        [Test]
        public void EventEqualsSucceededTest3()
        {
            DateTime lastUpdate = DateTime.Now;
            var event1 = new EventModel();
            event1.Id = "TestId";
            event1.LastUpdated = lastUpdate;
            event1.Originator = "TestOriginator";
            event1.Name = "TestName";
            event1.Description = "TestDescription";
            event1.State = EventState.Open;
            event1.Permission = SchedulePermissionToKnow.All;
            event1.Channel = "TestChannel";
            event1.Guild = "TestGuild";
            event1.MinEntry = 1;
            event1.MaxEntry = 5;
            event1.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event1.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event1.Editors = new List<string>() { "TestEditor2", "TestEditor1" };
            event1.Members = new List<string>() { "TestMembers2", "TestMembers1" };
            event1.Dates.Add(lastUpdate);
            event1.Dates.Add(lastUpdate + TimeSpan.FromDays(1));
            event1.Schedules.Add("key1", new Schedule());
            event1.Schedules["key1"].UserId = "Key1Id";
            event1.Schedules["key1"].Name = "Key1Name";
            event1.Schedules["key1"].Comment = "Key1Comment";
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            event1.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));
            event1.Schedules.Add("key2", new Schedule());
            event1.Schedules["key2"].UserId = "Key2Id";
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));
            event1.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));

            var event2 = new EventModel();
            event2.Id = "TestId";
            event2.LastUpdated = lastUpdate;
            event2.Originator = "TestOriginator";
            event2.Name = "TestName";
            event2.Description = "TestDescription";
            event2.State = EventState.Open;
            event2.Permission = SchedulePermissionToKnow.All;
            event2.Channel = "TestChannel";
            event2.Guild = "TestGuild";
            event2.MinEntry = 1;
            event2.MaxEntry = 5;
            event2.FixDate = lastUpdate + TimeSpan.FromDays(1);
            event2.HashedPassword = "E1FC309C5B67FC16F4CA816961498061ABB5B879B98AA4F03131EA6277FCA9EA";
            event2.Editors = new List<string>() { "TestEditor1", "TestEditor2" };
            event2.Members = new List<string>() { "TestMembers1", "TestMembers2" };
            event2.Dates.Add(lastUpdate + TimeSpan.FromDays(1));
            event2.Dates.Add(lastUpdate);
            event2.Schedules.Add("key2", new Schedule());
            event2.Schedules["key2"].UserId = "Key2Id";
            event2.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Hope));
            event2.Schedules["key2"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Present));
            event2.Schedules.Add("key1", new Schedule());
            event2.Schedules["key1"].UserId = "Key1Id";
            event2.Schedules["key1"].Name = "Key1Name";
            event2.Schedules["key1"].Comment = "Key1Comment";
            event2.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today, AttendanceStatus.Absent));
            event2.Schedules["key1"].DateStatuses.Add(new DateStatus(DateTime.Today + TimeSpan.FromDays(1), AttendanceStatus.Difficult));
            
            bool result = event1.ValueEquals(event2);
            result.IsTrue();
        }
    }
}