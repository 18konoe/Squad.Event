using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SquadEvent.Server.Controllers;
using SquadEvent.Server.Services;
using SquadEvent.Shared.Models;
using SquadEvent.Shared.Parameters;

namespace SquadEvent.Test
{
    [TestFixture]
    public class EventControllerTest
    {
        private ClaimsPrincipal _user;
        private EventTestData _testData;
        private readonly DateTimeOffset _today = DateTimeOffset.Now;
        private ControllerContext _context;
        private string _id = "1234567890ABCDEF";
        private string _userId = "1234567890";
        private string _email = "test@test.com";
        private string _name = "TestUser";
        private string _phase = "0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF";

        [SetUp]
        public void SetUp()
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.Name, _name),
                new Claim(ClaimTypes.Email, _email),
                new Claim(ClaimTypes.NameIdentifier, _userId)
            }));

            _context = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            _testData = new EventTestData(DateTime.Now.GetHashCode(), _today);
        }

        [Test]
        public void CreateEventTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object);

            var ev = new EventModel()
            {
                Originator = _userId,
                State = EventState.Draft
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                State = EventState.Draft
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest2_2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId + "a",
                State = EventState.Draft
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest3()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                State = EventState.Fixed
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest4()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                State = EventState.Closed
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest5()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                State = EventState.Open
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest6()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                State = EventState.Open,
                Name = "Test"
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest7()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today }
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest8()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => e);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test"
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsNull();
            result.Value.Id.Is(_id);
        }

        [Test]
        public void CreateEventTest9()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => e);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Draft
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsNull();
            result.Value.Id.Is(_id);
        }

        [Test]
        public void CreateEventTest10()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => e);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                FixDate = _today
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest11()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => e);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                MinEntry = 10
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsNull();
            result.Value.Id.Is(_id);
        }

        [Test]
        public void CreateEventTest12()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => e);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                MaxEntry = 10
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsNull();
            result.Value.Id.Is(_id);
        }

        [Test]
        public void CreateEventTest13()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => e);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                MinEntry = 10,
                MaxEntry = 10
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsNull();
            result.Value.Id.Is(_id);
        }

        [Test]
        public void CreateEventTest14()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => e);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                MinEntry = 10,
                MaxEntry = 9
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest15()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => e);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                Schedules = new Dictionary<string, Schedule>() { { "1111111111", new Schedule() } }
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest16()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test"
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void CreateEventTest17()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(service => service.AddEvent(It.IsAny<EventModel>())).Returns<EventModel>(e => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Id = _id,
                Originator = _userId,
                State = EventState.Draft
            };

            var result = target.CreateEvent(ev);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void GetEventTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object);

            var result = target.GetEvent(_id);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void GetEventTest2()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.GetEventById(It.IsAny<string>())).Returns<string>(str => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var result = target.GetEvent(_id);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void GetEventTest3()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.GetEventById(It.IsAny<string>())).Returns<string>(str => new EventModel());
            mock.Setup(s => s.IsPasswordRequired(It.IsAny<string>(), It.IsAny<EventModel>())).Returns<string, EventModel>((s, ev) => false);
            mock.Setup(s => s.FilterSchedule(It.IsAny<string>(), It.IsAny<EventModel>())).Returns<string, EventModel>((s, ev) => ev);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var result = target.GetEvent(_id);
            result.IsNotNull();
            result.Result.IsNull();
            result.Value.IsInstanceOf<EventModel>();
        }

        [Test]
        public void GetEventTest4()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.GetEventById(It.IsAny<string>())).Returns<string>(str => new EventModel());
            mock.Setup(s => s.IsPasswordRequired(It.IsAny<string>(), It.IsAny<EventModel>())).Returns<string, EventModel>((s, ev) => true);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.GetEvent(_id);
            result.IsNotNull();
            result.Result.IsInstanceOf<UnauthorizedResult>();
            result.Value.IsNull();
        }

        [Test]
        public void GetEventWithPasswordTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object);

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.GetEventWithPassword(param);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void GetEventWithPasswordTest2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = "WrongUserId",
                Phrase = _phase
            };

            var result = target.GetEventWithPassword(param);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void GetEventWithPasswordTest3()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.GetEventById(It.IsAny<string>())).Returns<string>(str => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.GetEventWithPassword(param);
            result.IsNotNull();
            result.Result.IsInstanceOf<BadRequestObjectResult>();
            result.Value.IsNull();
        }

        [Test]
        public void GetEventWithPasswordTest4()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.GetEventById(It.IsAny<string>())).Returns<string>(str => new EventModel());
            mock.Setup(s => s.IsPasswordRequired(It.IsAny<string>(), It.IsAny<EventModel>())).Returns<string, EventModel>((s, ev) => false);
            mock.Setup(s => s.FilterSchedule(It.IsAny<string>(), It.IsAny<EventModel>())).Returns<string, EventModel>((s, ev) => ev);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.GetEventWithPassword(param);
            result.IsNotNull();
            result.Result.IsNull();
            result.Value.IsInstanceOf<EventModel>();
        }

        [Test]
        public void GetEventWithPasswordTest5()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.GetEventById(It.IsAny<string>())).Returns<string>(str => new EventModel());
            mock.Setup(s => s.IsPasswordRequired(It.IsAny<string>(), It.IsAny<EventModel>())).Returns<string, EventModel>((s, ev) => true);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.GetEventWithPassword(param);
            result.IsNotNull();
            result.Result.IsInstanceOf<UnauthorizedResult>();
            result.Value.IsNull();
        }

        [Test]
        public void GetEventWithPasswordTest6()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.GetEventById(It.IsAny<string>())).Returns<string>(str => new EventModel() { HashedPassword = _phase });
            mock.Setup(s => s.IsPasswordRequired(It.IsAny<string>(), It.IsAny<EventModel>())).Returns<string, EventModel>((s, ev) => true);
            mock.Setup(s => s.FilterSchedule(It.IsAny<string>(), It.IsAny<EventModel>())).Returns<string, EventModel>((s, ev) => ev);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.GetEventWithPassword(param);
            result.IsNotNull();
            result.Result.IsNull();
            result.Value.IsInstanceOf<EventModel>();
        }

        [Test]
        public void UpdateEventTest0()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object);

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = null
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void UpdateEventTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object);

            var ev = new EventModel();
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void UpdateEventTest2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel();
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = "WrongUser",
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void UpdateEventTest3()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 9
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void UpdateEventTestForDraft1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Draft
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForOpenTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Open
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForOpenTest2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today }
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForOpenTest3()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.UpdateEvent(It.IsAny<EventIdentifyParameter>(), It.IsAny<EventModel>())).Returns<EventIdentifyParameter, EventModel>((param, ev) => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test"
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForOpenTest4()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.UpdateEvent(It.IsAny<EventIdentifyParameter>(), It.IsAny<EventModel>())).Returns<EventIdentifyParameter, EventModel>((param, ev) => ev);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Open,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test"
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<OkResult>();
        }

        [Test]
        public void PatchEventForFixedTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Fixed
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForFixedTest2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Fixed,
                Dates = new List<DateTimeOffset>() { _today }
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForFixedTest3()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Fixed,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test"
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForFixedTest4()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Fixed,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                Schedules = new Dictionary<string, Schedule>() { { "Test", new Schedule()} }
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForFixedTest5()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.UpdateEvent(It.IsAny<EventIdentifyParameter>(), It.IsAny<EventModel>())).Returns<EventIdentifyParameter, EventModel>((param, ev) => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Fixed,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                Schedules = new Dictionary<string, Schedule>() { { "Test", new Schedule() } },
                FixDate = _today
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForFixedTest6()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.UpdateEvent(It.IsAny<EventIdentifyParameter>(), It.IsAny<EventModel>())).Returns<EventIdentifyParameter, EventModel>((param, ev) => ev);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Fixed,
                Dates = new List<DateTimeOffset>() { _today },
                Name = "Test",
                Schedules = new Dictionary<string, Schedule>() { { "Test", new Schedule() } },
                FixDate = _today
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<OkResult>();
        }

        [Test]
        public void PatchEventForClosedTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Closed
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForClosedTest2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Closed,
                FixDate = _today + TimeSpan.FromDays(1)
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForClosedTest3()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.UpdateEvent(It.IsAny<EventIdentifyParameter>(), It.IsAny<EventModel>())).Returns<EventIdentifyParameter, EventModel>((param, ev) => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Closed,
                FixDate = _today
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void PatchEventForClosedTest4()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.UpdateEvent(It.IsAny<EventIdentifyParameter>(), It.IsAny<EventModel>())).Returns<EventIdentifyParameter, EventModel>((param, ev) => ev);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var ev = new EventModel()
            {
                Originator = _userId,
                MinEntry = 10,
                MaxEntry = 10,
                State = EventState.Closed,
                FixDate = _today
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedEvent = ev
            };

            var result = target.UpdateEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<OkResult>();
        }

        [Test]
        public void DeleteEventTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object);

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.DeleteEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void DeleteEventTest2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = "WrongUserId",
                Phrase = _phase
            };

            var result = target.DeleteEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void DeleteEventTest3()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.DeleteEvent(It.IsAny<EventIdentifyParameter>())).Returns<EventIdentifyParameter>(param => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.DeleteEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void DeleteEventTest4()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.DeleteEvent(It.IsAny<EventIdentifyParameter>())).Returns<EventIdentifyParameter>(param => new EventModel());
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase
            };

            var result = target.DeleteEvent(param);
            result.IsNotNull();
            result.IsInstanceOf<OkResult>();
        }

        [Test]
        public void AddOrUpdateScheduleTest0()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object);
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedSchedule = null
            };

            var result = target.AddOrUpdateSchedule(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void AddOrUpdateScheduleTest1()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object);

            var schedule = new Schedule();
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedSchedule = schedule
            };

            var result = target.AddOrUpdateSchedule(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void AddOrUpdateScheduleTest2()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var schedule = new Schedule();
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = "WrongUser",
                Phrase = _phase,
                AttachedSchedule = schedule
            };

            var result = target.AddOrUpdateSchedule(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void AddOrUpdateScheduleTest3()
        {
            var mock = new Mock<IEventService>();
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var schedule = new Schedule()
            {
                UserId = "WrongUser"
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedSchedule = schedule
            };

            var result = target.AddOrUpdateSchedule(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void AddOrUpdateScheduleTest4()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.AddOrUpdateSchedule(It.IsAny<EventIdentifyParameter>(), It.IsAny<Schedule>())).Returns<EventIdentifyParameter, Schedule>((param, sc) => null);
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var schedule = new Schedule()
            {
                UserId = _userId
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedSchedule = schedule
            };

            var result = target.AddOrUpdateSchedule(param);
            result.IsNotNull();
            result.IsInstanceOf<BadRequestObjectResult>();
        }

        [Test]
        public void AddOrUpdateScheduleTest5()
        {
            var mock = new Mock<IEventService>();
            mock.Setup(s => s.AddOrUpdateSchedule(It.IsAny<EventIdentifyParameter>(), It.IsAny<Schedule>())).Returns<EventIdentifyParameter, Schedule>((param, sc) => new EventModel());
            var target = new EventController(mock.Object)
            {
                ControllerContext = _context
            };

            var schedule = new Schedule()
            {
                UserId = _userId
            };
            var param = new EventIdentifyParameter()
            {
                Id = _id,
                UserId = _userId,
                Phrase = _phase,
                AttachedSchedule = schedule
            };

            var result = target.AddOrUpdateSchedule(param);
            result.IsNotNull();
            result.IsInstanceOf<OkResult>();
        }
    }
}