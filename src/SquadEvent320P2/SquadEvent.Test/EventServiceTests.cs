using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using SquadEvent.Server.Model;
using SquadEvent.Server.Services;
using SquadEvent.Shared.Models;
using SquadEvent.Shared.Parameters;

namespace SquadEvent.Test
{
    [TestFixture]
    public class EventServiceTests
    {
        private IEventsStoreDatabaseSettings _settings;
        private EventService _service;
        private EventTestData _testData;
        private IMongoCollection<EventModel> _events;
        private readonly DateTimeOffset _today = DateTimeOffset.Now;

        [SetUp]
        public void SetUp()
        {
            var mock = new Mock<IEventsStoreDatabaseSettings>();
            mock.SetupGet(x => x.EventsCollectionName).Returns("TestEventCollection");
            mock.SetupGet(x => x.ConnectionString).Returns("mongodb://localhost:27017");
            mock.SetupGet(x => x.DatabaseName).Returns("TestEventStoreDb");
            _settings = mock.Object;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _events = database.GetCollection<EventModel>(_settings.EventsCollectionName);
            _service = new EventService(new EventCollectionProvider(_settings));
            _testData = new EventTestData(DateTime.Now.GetHashCode(), _today);

            _events.InsertMany(_testData.AllTestData);
        }

        [Test]
        public void AddEventTest1()
        {
            _events.DeleteMany(ev => true);

            var result1 = _service.AddEvent(_testData.JustNewEvent);
            result1.IsNotNull();
            string.IsNullOrEmpty(result1.Id).IsFalse();
            
            var result2 = _service.AddEvent(_testData.InitialFillEvent);
            result2.IsNotNull();
            string.IsNullOrEmpty(result2.Id).IsFalse();

            var result3 = _service.AddEvent(_testData.BasicDraftEvent1);
            result3.IsNotNull();
            string.IsNullOrEmpty(result3.Id).IsFalse();

            var result4 = _service.AddEvent(_testData.BasicDraftEvent2);
            result4.IsNotNull();
            string.IsNullOrEmpty(result4.Id).IsFalse();

            var result5 = _service.AddEvent(_testData.BasicOpenEvent1);
            result5.IsNotNull();
            string.IsNullOrEmpty(result5.Id).IsFalse();

            var result6 = _service.AddEvent(_testData.BasicOpenEvent2);
            result6.IsNotNull();
            string.IsNullOrEmpty(result6.Id).IsFalse();

            var result7 = _service.AddEvent(_testData.BasicFixedEvent);
            result7.IsNotNull();
            string.IsNullOrEmpty(result7.Id).IsFalse();

            _events.CountDocuments(ev => true).Is(7);
        }

        [Test]
        public void AddEventTest2()
        {
            var collectionMock = new Mock<IMongoCollection<EventModel>>();
            collectionMock.Setup(collection => collection.InsertOne(It.IsAny<EventModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>())).Throws(new Exception());
            var providerMock = new Mock<IMongoCollectionProvider<EventModel>>();
            providerMock.Setup(provider => provider.GetCollection()).Returns(collectionMock.Object);
            var service = new EventService(providerMock.Object);

            service.AddEvent(_testData.JustNewEvent).IsNull();
        }

        [Test]
        public void GetEventByIdTest1()
        {
            _service.GetEventById(_testData.JustNewEvent.Id).ValueEquals(_testData.JustNewEvent).IsTrue();
            _service.GetEventById(_testData.InitialFillEvent.Id).ValueEquals(_testData.InitialFillEvent).IsTrue();
            _service.GetEventById(_testData.BasicDraftEvent1.Id).ValueEquals(_testData.BasicDraftEvent1).IsTrue();
            _service.GetEventById(_testData.BasicDraftEvent2.Id).ValueEquals(_testData.BasicDraftEvent2).IsTrue();
            _service.GetEventById(_testData.BasicOpenEvent1.Id).ValueEquals(_testData.BasicOpenEvent1).IsTrue();
            _service.GetEventById(_testData.BasicOpenEvent2.Id).ValueEquals(_testData.BasicOpenEvent2).IsTrue();
            _service.GetEventById(_testData.BasicFixedEvent.Id).ValueEquals(_testData.BasicFixedEvent).IsTrue();
        }

        [Test]
        public void GetEventsByOriginatorIdTest()
        {
            var events1 = _service.GetEventsByOriginatorId(string.Empty);
            events1.Count.Is(2);
            var events2 = _service.GetEventsByOriginatorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.BasicDraftEvent1)}"]);
            events2.Count.Is(2);
            var events3 = _service.GetEventsByOriginatorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.BasicDraftEvent2)}"]);
            events3.Count.Is(0);
            var events4 = _service.GetEventsByOriginatorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.BasicOpenEvent1)}"]);
            events4.Count.Is(2);
            var events5 = _service.GetEventsByOriginatorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.BasicOpenEvent2)}"]);
            events5.Count.Is(0);
            var events6 = _service.GetEventsByOriginatorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.BasicFixedEvent)}"]);
            events6.Count.Is(1);
            var events7 = _service.GetEventsByOriginatorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"]);
            events7.Count.Is(4);

        }

        [Test]
        public void GetEventsByEditorIdTest()
        {
            var events1 = _service.GetEventsByEditorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"]);
            events1.Count.Is(1);
            var events2 = _service.GetEventsByEditorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"]);
            events2.Count.Is(2);
            var events3 = _service.GetEventsByEditorId(_testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"]);
            events3.Count.Is(6);
        }

        [Test]
        public void GetEventsByInputUserIdTest()
        {
            var events0 = _service.GetEventsByInputUserId(_testData.OriginatorIdDictionary["InputUserExample0"]);
            events0.Count.Is(0);

            var events1 = _service.GetEventsByInputUserId(_testData.OriginatorIdDictionary["InputUserExample1"]);
            events1.Count.Is(1);

            var events2 = _service.GetEventsByInputUserId(_testData.OriginatorIdDictionary["InputUserExample2"]);
            events2.Count.Is(2);

            var events3 = _service.GetEventsByInputUserId(_testData.OriginatorIdDictionary["InputUserExample3"]);
            events3.Count.Is(3);

            var events4 = _service.GetEventsByInputUserId(_testData.OriginatorIdDictionary["InputUserExample4"]);
            events4.Count.Is(4);

            var events5 = _service.GetEventsByInputUserId(_testData.OriginatorIdDictionary["InputUserExample5"]);
            events5.Count.Is(6);
        }

        [Test]
        public void GetEventUpdateByParamTest1()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = "WrongPassword"
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventUpdateByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventUpdateByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventUpdateByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventUpdateByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventUpdateByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventUpdateByParamTest2()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = null
        };

            param.UserId = ownerId;
            var result1 = _service.GetEventUpdateByParam(param);
            result1.IsNotNull();

            param.UserId = editorId;
            var result2 = _service.GetEventUpdateByParam(param);
            result2.IsNotNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventUpdateByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventUpdateByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventUpdateByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventUpdateByParamTest3()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = "WrongPassword"
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventUpdateByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventUpdateByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventUpdateByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventUpdateByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventUpdateByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventUpdateByParamTest4()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = null
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventUpdateByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventUpdateByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventUpdateByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventUpdateByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventUpdateByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventUpdateByParamTest5()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = _testData.TestPasswordProvider.ToString()
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventUpdateByParam(param);
            result1.IsNotNull();

            param.UserId = editorId;
            var result2 = _service.GetEventUpdateByParam(param);
            result2.IsNotNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventUpdateByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventUpdateByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventUpdateByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventDeleteByParamTest1()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = "WrongPassword"
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventDeleteByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventDeleteByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventDeleteByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventDeleteByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventDeleteByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventDeleteByParamTest2()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = null
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventDeleteByParam(param);
            result1.IsNotNull();

            param.UserId = editorId;
            var result2 = _service.GetEventDeleteByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventDeleteByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventDeleteByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventDeleteByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventDeleteByParamTest3()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = "WrongPassword"
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventDeleteByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventDeleteByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventDeleteByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventDeleteByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventDeleteByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventDeleteByParamTest4()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = null
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventDeleteByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventDeleteByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventDeleteByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventDeleteByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventDeleteByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventDeleteByParamTest5()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = _testData.TestPasswordProvider.ToString()
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventDeleteByParam(param);
            result1.IsNotNull();

            param.UserId = editorId;
            var result2 = _service.GetEventDeleteByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventDeleteByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventDeleteByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventDeleteByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventScheduleUpdateByParamTest1()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = "WrongPassword"
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventScheduleUpdateByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventScheduleUpdateByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventScheduleUpdateByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventScheduleUpdateByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventScheduleUpdateByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventScheduleUpdateByParamTest2()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = null
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventScheduleUpdateByParam(param);
            result1.IsNotNull();

            param.UserId = editorId;
            var result2 = _service.GetEventScheduleUpdateByParam(param);
            result2.IsNotNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventScheduleUpdateByParam(param);
            result3.IsNotNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventScheduleUpdateByParam(param);
            result4.IsNotNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventScheduleUpdateByParam(param);
            result5.IsNotNull();
        }

        [Test]
        public void GetEventScheduleUpdateByParamTest3()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = "WrongPassword"
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventScheduleUpdateByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventScheduleUpdateByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventScheduleUpdateByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventScheduleUpdateByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventScheduleUpdateByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventScheduleUpdateByParamTest4()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = null
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventScheduleUpdateByParam(param);
            result1.IsNull();

            param.UserId = editorId;
            var result2 = _service.GetEventScheduleUpdateByParam(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventScheduleUpdateByParam(param);
            result3.IsNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventScheduleUpdateByParam(param);
            result4.IsNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventScheduleUpdateByParam(param);
            result5.IsNull();
        }

        [Test]
        public void GetEventScheduleUpdateByParamTest5()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = _testData.TestPasswordProvider.ToString()
            };

            param.UserId = ownerId;
            var result1 = _service.GetEventScheduleUpdateByParam(param);
            result1.IsNotNull();

            param.UserId = editorId;
            var result2 = _service.GetEventScheduleUpdateByParam(param);
            result2.IsNotNull();

            param.UserId = memberUserId;
            var result3 = _service.GetEventScheduleUpdateByParam(param);
            result3.IsNotNull();

            param.UserId = inputUserId;
            var result4 = _service.GetEventScheduleUpdateByParam(param);
            result4.IsNotNull();

            param.UserId = noRelationId;
            var result5 = _service.GetEventScheduleUpdateByParam(param);
            result5.IsNotNull();
        }

        [Test]
        public void AddOrUpdateScheduleTest1()
        {
            var userId = _testData.OriginatorIdDictionary["UpdateUserExample"];
            var eventId = _testData.EditorExistEvent1.Id;
            var initial = _events.Find(ev => ev.Id == eventId).ToList().Single();
            initial.Schedules.Count().Is(1);

            var updateSchedule1 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample"
            };
            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                UserId = userId,
                Phrase = "WrongPassword"
            };

            var result1 = _service.AddOrUpdateSchedule(param, updateSchedule1);
            result1.IsNull();

            var updated1 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated1.Schedules.Count().Is(1);
        }

        [Test]
        public void AddOrUpdateScheduleTest2()
        {
            var userId = _testData.OriginatorIdDictionary["UpdateUserExample"];
            var eventId = _testData.EditorExistEvent1.Id;
            var initial = _events.Find(ev => ev.Id == eventId).ToList().Single();
            initial.Schedules.Count().Is(1);

            var updateSchedule1 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample"
            };
            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                UserId = userId,
                Phrase = null
            };

            var result1 = _service.AddOrUpdateSchedule(param, updateSchedule1);
            result1.IsNotNull();

            var updated1 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated1.Schedules.Count().Is(2);
            updated1.Schedules[userId].UserId.Is(userId);
            updated1.Schedules[userId].Name.Is("UpdateUserExample");
            updated1.Schedules[userId].DateStatuses.Count.Is(0);

            var updateSchedule2 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample2",
                DateStatuses = new List<DateStatus>()
                {
                    new DateStatus(_today, AttendanceStatus.Present)
                }
            };

            var result2 = _service.AddOrUpdateSchedule(param, updateSchedule2);
            result2.IsNotNull();

            var updated2 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated2.Schedules.Count().Is(2);
            updated2.Schedules[userId].UserId.Is(userId);
            updated2.Schedules[userId].Name.Is("UpdateUserExample2");
            updated2.Schedules[userId].DateStatuses.Count.Is(1);
            updated2.Schedules[userId].DateStatuses[0].Date.Is(_today);
            updated2.Schedules[userId].DateStatuses[0].Status.Is(AttendanceStatus.Present);

            var updateSchedule3 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample3",
                DateStatuses = new List<DateStatus>()
                {
                    new DateStatus(_today, AttendanceStatus.Absent)
                }
            };

            var result3 = _service.AddOrUpdateSchedule(param, updateSchedule3);
            result3.IsNotNull();

            var updated3 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated3.Schedules.Count().Is(2);
            updated3.Schedules[userId].UserId.Is(userId);
            updated3.Schedules[userId].Name.Is("UpdateUserExample3");
            updated3.Schedules[userId].DateStatuses.Count.Is(1);
            updated3.Schedules[userId].DateStatuses[0].Date.Is(_today);
            updated3.Schedules[userId].DateStatuses[0].Status.Is(AttendanceStatus.Absent);
        }

        [Test]
        public void AddOrUpdateScheduleTest3()
        {
            var userId = _testData.OriginatorIdDictionary["UpdateUserExample"];
            var eventId = _testData.EditorExistEvent1P.Id;
            var initial = _events.Find(ev => ev.Id == eventId).ToList().Single();
            initial.Schedules.Count().Is(1);

            var updateSchedule1 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample"
            };
            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                UserId = userId,
                Phrase = null
            };

            var result1 = _service.AddOrUpdateSchedule(param, updateSchedule1);
            result1.IsNull();

            var updated1 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated1.Schedules.Count().Is(1);
        }

        [Test]
        public void AddOrUpdateScheduleTest4()
        {
            var userId = _testData.OriginatorIdDictionary["UpdateUserExample"];
            var eventId = _testData.EditorExistEvent1P.Id;
            var initial = _events.Find(ev => ev.Id == eventId).ToList().Single();
            initial.Schedules.Count().Is(1);

            var updateSchedule1 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample"
            };
            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                UserId = userId,
                Phrase = "WrongPassword"
            };

            var result1 = _service.AddOrUpdateSchedule(param, updateSchedule1);
            result1.IsNull();

            var updated1 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated1.Schedules.Count().Is(1);
        }

        [Test]
        public void AddOrUpdateScheduleTest5()
        {
            var userId = _testData.OriginatorIdDictionary["UpdateUserExample"];
            var eventId = _testData.EditorExistEvent1P.Id;
            var initial = _events.Find(ev => ev.Id == eventId).ToList().Single();
            initial.Schedules.Count().Is(1);

            var updateSchedule1 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample",
                Comment = "UpdateUserExample Comment"
            };
            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                UserId = userId,
                Phrase = _testData.TestPasswordProvider.ToString()
            };

            var result1 = _service.AddOrUpdateSchedule(param, updateSchedule1);
            result1.IsNotNull();

            var updated1 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated1.Schedules.Count().Is(2);
            updated1.Schedules[userId].UserId.Is(userId);
            updated1.Schedules[userId].Name.Is("UpdateUserExample");
            updated1.Schedules[userId].Comment.Is("UpdateUserExample Comment");
            updated1.Schedules[userId].DateStatuses.Count.Is(0);

            var updateSchedule2 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample2",
                Comment = "UpdateUserExample2 Comment",
                DateStatuses = new List<DateStatus>()
                {
                    new DateStatus(_today, AttendanceStatus.Present)
                }
            };

            var result2 = _service.AddOrUpdateSchedule(param, updateSchedule2);
            result2.IsNotNull();

            var updated2 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated2.Schedules.Count().Is(2);
            updated2.Schedules[userId].UserId.Is(userId);
            updated2.Schedules[userId].Name.Is("UpdateUserExample2");
            updated2.Schedules[userId].Comment.Is("UpdateUserExample2 Comment");
            updated2.Schedules[userId].DateStatuses.Count.Is(1);
            updated2.Schedules[userId].DateStatuses[0].Date.Is(_today);
            updated2.Schedules[userId].DateStatuses[0].Status.Is(AttendanceStatus.Present);

            var updateSchedule3 = new Schedule()
            {
                UserId = userId,
                Name = "UpdateUserExample3",
                DateStatuses = new List<DateStatus>()
                {
                    new DateStatus(_today, AttendanceStatus.Absent)
                }
            };

            var result3 = _service.AddOrUpdateSchedule(param, updateSchedule3);
            result3.IsNotNull();

            var updated3 = _events.Find(ev => ev.Id == eventId).ToList().Single();
            updated3.Schedules.Count().Is(2);
            updated3.Schedules[userId].UserId.Is(userId);
            updated3.Schedules[userId].Name.Is("UpdateUserExample3");
            updated3.Schedules[userId].Comment.Is(string.Empty);
            updated3.Schedules[userId].DateStatuses.Count.Is(1);
            updated3.Schedules[userId].DateStatuses[0].Date.Is(_today);
            updated3.Schedules[userId].DateStatuses[0].Status.Is(AttendanceStatus.Absent);
        }

        [Test]
        public void UpdateEventTest1()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = "WrongPassword"
            };

            param.UserId = noRelationId;
            var result1 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result4.IsNull();

            param.UserId = ownerId;
            var result5 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result5.IsNull();
        }

        [Test]
        public void UpdateEventTest2()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = null
            };

            param.UserId = noRelationId;
            var result1 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result4.IsNotNull();

            var updated1 = _service.GetEventById(param.Id);
            updated1.IsNotNull();
            updated1.Name.Is(_testData.UpdatedEvent.Name);
            updated1.Description.Is(_testData.UpdatedEvent.Description);
            updated1.Dates.IsStructuralEqual(_testData.UpdatedEvent.Dates);
            updated1.State.Is(_testData.UpdatedEvent.State);
            updated1.Originator.IsNot(_testData.UpdatedEvent.Originator);
            updated1.Schedules.IsNotStructuralEqual(_testData.UpdatedEvent.Schedules);
            updated1.Permission.Is(_testData.UpdatedEvent.Permission);
            updated1.LastUpdated.IsNot(_testData.UpdatedEvent.LastUpdated);
            updated1.Editors.IsStructuralEqual(_testData.UpdatedEvent.Editors);
            updated1.Members.IsStructuralEqual(_testData.UpdatedEvent.Members);
            updated1.Guild.Is(_testData.UpdatedEvent.Guild);
            updated1.Channel.Is(_testData.UpdatedEvent.Channel);
            updated1.MinEntry.Is(_testData.UpdatedEvent.MinEntry);
            updated1.MaxEntry.Is(_testData.UpdatedEvent.MaxEntry);
            updated1.FixDate.Is(_testData.UpdatedEvent.FixDate);
            updated1.HashedPassword.Is(_testData.UpdatedEvent.HashedPassword);
        }

        [Test]
        public void UpdateEventTest3()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1.Id,
                Phrase = null
            };

            param.UserId = noRelationId;
            var result1 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result3.IsNull();

            param.UserId = ownerId;
            var result4 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result4.IsNotNull();

            var updated1 = _service.GetEventById(param.Id);
            updated1.IsNotNull();
            updated1.Name.Is(_testData.UpdatedEvent.Name);
            updated1.Dates.IsStructuralEqual(_testData.UpdatedEvent.Dates);
            updated1.State.Is(_testData.UpdatedEvent.State);
            updated1.Originator.IsNot(_testData.UpdatedEvent.Originator);
            updated1.Schedules.IsNotStructuralEqual(_testData.UpdatedEvent.Schedules);
            updated1.Permission.Is(_testData.UpdatedEvent.Permission);
            updated1.LastUpdated.IsNot(_testData.UpdatedEvent.LastUpdated);
            updated1.Editors.IsStructuralEqual(_testData.UpdatedEvent.Editors);
            updated1.Members.IsStructuralEqual(_testData.UpdatedEvent.Members);
            updated1.Guild.Is(_testData.UpdatedEvent.Guild);
            updated1.Channel.Is(_testData.UpdatedEvent.Channel);
            updated1.MinEntry.Is(_testData.UpdatedEvent.MinEntry);
            updated1.MaxEntry.Is(_testData.UpdatedEvent.MaxEntry);
            updated1.FixDate.Is(_testData.UpdatedEvent.FixDate);
            updated1.HashedPassword.Is(_testData.UpdatedEvent.HashedPassword);
        }

        [Test]
        public void UpdateEventTest4()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = null
            };

            param.UserId = noRelationId;
            var result1 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result4.IsNull();

            param.UserId = ownerId;
            var result5 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result5.IsNull();
        }

        [Test]
        public void UpdateEventTest5()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = "WrongPassword"
            };

            param.UserId = noRelationId;
            var result1 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result4.IsNull();

            param.UserId = ownerId;
            var result5 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result5.IsNull();
        }

        [Test]
        public void UpdateEventTest6()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = _testData.TestPasswordProvider.ToString()
            };

            param.UserId = noRelationId;
            var result1 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result4.IsNotNull();

            var updated1 = _service.GetEventById(param.Id);
            updated1.IsNotNull();
            updated1.Name.Is(_testData.UpdatedEvent.Name);
            updated1.Dates.IsStructuralEqual(_testData.UpdatedEvent.Dates);
            updated1.State.Is(_testData.UpdatedEvent.State);
            updated1.Originator.IsNot(_testData.UpdatedEvent.Originator);
            updated1.Schedules.IsNotStructuralEqual(_testData.UpdatedEvent.Schedules);
            updated1.Permission.Is(_testData.UpdatedEvent.Permission);
            updated1.LastUpdated.IsNot(_testData.UpdatedEvent.LastUpdated);
            updated1.Editors.IsStructuralEqual(_testData.UpdatedEvent.Editors);
            updated1.Members.IsStructuralEqual(_testData.UpdatedEvent.Members);
            updated1.Guild.Is(_testData.UpdatedEvent.Guild);
            updated1.Channel.Is(_testData.UpdatedEvent.Channel);
            updated1.MinEntry.Is(_testData.UpdatedEvent.MinEntry);
            updated1.MaxEntry.Is(_testData.UpdatedEvent.MaxEntry);
            updated1.FixDate.Is(_testData.UpdatedEvent.FixDate);
            updated1.HashedPassword.Is(_testData.UpdatedEvent.HashedPassword);
        }

        [Test]
        public void UpdateEventTest7()
        {
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = _testData.EditorExistEvent1P.Id,
                Phrase = _testData.TestPasswordProvider.ToString()
            };

            param.UserId = noRelationId;
            var result1 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result3.IsNull();

            param.UserId = ownerId;
            var result4 = _service.UpdateEvent(param, _testData.UpdatedEvent);
            result4.IsNotNull();

            var updated1 = _service.GetEventById(param.Id);
            updated1.IsNotNull();
            updated1.Name.Is(_testData.UpdatedEvent.Name);
            updated1.Dates.IsStructuralEqual(_testData.UpdatedEvent.Dates);
            updated1.State.Is(_testData.UpdatedEvent.State);
            updated1.Originator.IsNot(_testData.UpdatedEvent.Originator);
            updated1.Schedules.IsNotStructuralEqual(_testData.UpdatedEvent.Schedules);
            updated1.Permission.Is(_testData.UpdatedEvent.Permission);
            updated1.LastUpdated.IsNot(_testData.UpdatedEvent.LastUpdated);
            updated1.Editors.IsStructuralEqual(_testData.UpdatedEvent.Editors);
            updated1.Members.IsStructuralEqual(_testData.UpdatedEvent.Members);
            updated1.Guild.Is(_testData.UpdatedEvent.Guild);
            updated1.Channel.Is(_testData.UpdatedEvent.Channel);
            updated1.MinEntry.Is(_testData.UpdatedEvent.MinEntry);
            updated1.MaxEntry.Is(_testData.UpdatedEvent.MaxEntry);
            updated1.FixDate.Is(_testData.UpdatedEvent.FixDate);
            updated1.HashedPassword.Is(_testData.UpdatedEvent.HashedPassword);
        }

        [Test]
        public void UpdateEventStateTest1()
        {
            _service.UpdateEventState(null, EventState.Open, _today).IsNull();
        }

        [Test]
        public void UpdateEventStateTest2()
        {
            _service.UpdateEventState(new EventIdentifyParameter(), EventState.Draft, _today).IsNull();
        }

        [Test]
        public void DeleteEventTest1()
        {
            var eventId = _testData.EditorExistEvent1.Id;
            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(1);

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                Phrase = "WrongPassword"
            };

            param.UserId = noRelationId;
            var result1 = _service.DeleteEvent(param);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.DeleteEvent(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.DeleteEvent(param);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.DeleteEvent(param);
            result4.IsNull();

            param.UserId = ownerId;
            var result5 = _service.DeleteEvent(param);
            result5.IsNull();

            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(1);
        }

        [Test]
        public void DeleteEventTest2()
        {
            var eventId = _testData.EditorExistEvent1.Id;
            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(1);

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                Phrase = null
            };

            param.UserId = noRelationId;
            var result1 = _service.DeleteEvent(param);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.DeleteEvent(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.DeleteEvent(param);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.DeleteEvent(param);
            result4.IsNull();

            param.UserId = ownerId;
            var result5 = _service.DeleteEvent(param);
            result5.IsNotNull();

            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(0);
        }

        [Test]
        public void DeleteEventTest3()
        {
            var eventId = _testData.EditorExistEvent1P.Id;
            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(1);

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                Phrase = "WrongPassword"
            };

            param.UserId = noRelationId;
            var result1 = _service.DeleteEvent(param);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.DeleteEvent(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.DeleteEvent(param);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.DeleteEvent(param);
            result4.IsNull();

            param.UserId = ownerId;
            var result5 = _service.DeleteEvent(param);
            result5.IsNull();

            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(1);
        }

        [Test]
        public void DeleteEventTest4()
        {
            var eventId = _testData.EditorExistEvent1P.Id;
            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(1);

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                Phrase = null
            };

            param.UserId = noRelationId;
            var result1 = _service.DeleteEvent(param);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.DeleteEvent(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.DeleteEvent(param);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.DeleteEvent(param);
            result4.IsNull();

            param.UserId = ownerId;
            var result5 = _service.DeleteEvent(param);
            result5.IsNull();

            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(1);
        }

        [Test]
        public void DeleteEventTest5()
        {
            var eventId = _testData.EditorExistEvent1P.Id;
            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(1);

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            EventIdentifyParameter param = new EventIdentifyParameter()
            {
                Id = eventId,
                Phrase = _testData.TestPasswordProvider.ToString()
            };

            param.UserId = noRelationId;
            var result1 = _service.DeleteEvent(param);
            result1.IsNull();

            param.UserId = inputUserId;
            var result2 = _service.DeleteEvent(param);
            result2.IsNull();

            param.UserId = memberUserId;
            var result3 = _service.DeleteEvent(param);
            result3.IsNull();

            param.UserId = editorId;
            var result4 = _service.DeleteEvent(param);
            result4.IsNull();

            param.UserId = ownerId;
            var result5 = _service.DeleteEvent(param);
            result5.IsNotNull();

            _events.Find(ev => ev.Id == eventId).ToList().Count.Is(0);
        }

        [Test]
        public void FilterScheduleTest1()
        {
            var ev1 = _testData.FilterScheduleTest1;
            var ev2 = _testData.FilterScheduleTest2;
            var ev3 = _testData.FilterScheduleTest3;
            var ev4 = _testData.FilterScheduleTest4;
            var ev5 = _testData.FilterScheduleTest5;

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            _service.FilterSchedule(noRelationId, ev1).Schedules.Count.Is(5);
            _service.FilterSchedule(inputUserId, ev2).Schedules.Count.Is(5);
            _service.FilterSchedule(memberUserId, ev3).Schedules.Count.Is(5);
            _service.FilterSchedule(editorId, ev4).Schedules.Count.Is(5);
            _service.FilterSchedule(ownerId, ev5).Schedules.Count.Is(5);
        }

        [Test]
        public void FilterScheduleTest2()
        {
            var ev1 = _testData.FilterScheduleTest1;
            var ev2 = _testData.FilterScheduleTest2;
            var ev3 = _testData.FilterScheduleTest3;
            var ev4 = _testData.FilterScheduleTest4;
            var ev5 = _testData.FilterScheduleTest5;

            ev1.Permission = SchedulePermissionToKnow.Input;
            ev2.Permission = SchedulePermissionToKnow.Input;
            ev3.Permission = SchedulePermissionToKnow.Input;
            ev4.Permission = SchedulePermissionToKnow.Input;
            ev5.Permission = SchedulePermissionToKnow.Input;

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            _service.FilterSchedule(noRelationId, ev1).Schedules.Count.Is(0);
            _service.FilterSchedule(inputUserId, ev2).Schedules.Count.Is(5);
            _service.FilterSchedule(memberUserId, ev3).Schedules.Count.Is(5);
            _service.FilterSchedule(editorId, ev4).Schedules.Count.Is(5);
            _service.FilterSchedule(ownerId, ev5).Schedules.Count.Is(5);
        }

        [Test]
        public void FilterScheduleTest3()
        {
            var ev1 = _testData.FilterScheduleTest1;
            var ev2 = _testData.FilterScheduleTest2;
            var ev3 = _testData.FilterScheduleTest3;
            var ev4 = _testData.FilterScheduleTest4;
            var ev5 = _testData.FilterScheduleTest5;

            ev1.Permission = SchedulePermissionToKnow.Member;
            ev2.Permission = SchedulePermissionToKnow.Member;
            ev3.Permission = SchedulePermissionToKnow.Member;
            ev4.Permission = SchedulePermissionToKnow.Member;
            ev5.Permission = SchedulePermissionToKnow.Member;

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            _service.FilterSchedule(noRelationId, ev1).Schedules.Count.Is(0);
            _service.FilterSchedule(inputUserId, ev2).Schedules.Count.Is(1);
            _service.FilterSchedule(memberUserId, ev3).Schedules.Count.Is(5);
            _service.FilterSchedule(editorId, ev4).Schedules.Count.Is(5);
            _service.FilterSchedule(ownerId, ev5).Schedules.Count.Is(5);
        }

        [Test]
        public void FilterScheduleTest4()
        {
            var ev1 = _testData.FilterScheduleTest1;
            var ev2 = _testData.FilterScheduleTest2;
            var ev3 = _testData.FilterScheduleTest3;
            var ev4 = _testData.FilterScheduleTest4;
            var ev5 = _testData.FilterScheduleTest5;

            ev1.Permission = SchedulePermissionToKnow.Editor;
            ev2.Permission = SchedulePermissionToKnow.Editor;
            ev3.Permission = SchedulePermissionToKnow.Editor;
            ev4.Permission = SchedulePermissionToKnow.Editor;
            ev5.Permission = SchedulePermissionToKnow.Editor;

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            _service.FilterSchedule(noRelationId, ev1).Schedules.Count.Is(0);
            _service.FilterSchedule(inputUserId, ev2).Schedules.Count.Is(1);
            _service.FilterSchedule(memberUserId, ev3).Schedules.Count.Is(1);
            _service.FilterSchedule(editorId, ev4).Schedules.Count.Is(5);
            _service.FilterSchedule(ownerId, ev5).Schedules.Count.Is(5);
        }

        [Test]
        public void FilterScheduleTest5()
        {
            var ev1 = _testData.FilterScheduleTest1;
            var ev2 = _testData.FilterScheduleTest2;
            var ev3 = _testData.FilterScheduleTest3;
            var ev4 = _testData.FilterScheduleTest4;
            var ev5 = _testData.FilterScheduleTest5;

            ev1.Permission = SchedulePermissionToKnow.Originator;
            ev2.Permission = SchedulePermissionToKnow.Originator;
            ev3.Permission = SchedulePermissionToKnow.Originator;
            ev4.Permission = SchedulePermissionToKnow.Originator;
            ev5.Permission = SchedulePermissionToKnow.Originator;

            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            _service.FilterSchedule(noRelationId, ev1).Schedules.Count.Is(0);
            _service.FilterSchedule(inputUserId, ev2).Schedules.Count.Is(1);
            _service.FilterSchedule(memberUserId, ev3).Schedules.Count.Is(1);
            _service.FilterSchedule(editorId, ev4).Schedules.Count.Is(1);
            _service.FilterSchedule(ownerId, ev5).Schedules.Count.Is(5);
        }

        [Test]
        public void IsPasswordRequiredTest1()
        {
            var ev = new EventModel();
            var result = _service.IsPasswordRequired("", ev);
            result.IsFalse();
        }

        [Test]
        public void IsPasswordRequiredTest2()
        {
            var ev = new EventModel(){HashedPassword = string.Empty};
            var result = _service.IsPasswordRequired("", ev);
            result.IsFalse();
        }

        [Test]
        public void IsPasswordRequiredTest3()
        {
            var ev = _testData.EditorExistEvent1P;
            var ownerId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent1)}"];
            var editorId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent3)}"];
            var memberUserId = _testData.OriginatorIdDictionary["MemberUserExample"];
            var inputUserId = _testData.OriginatorIdDictionary["InputUserExample5"];
            var noRelationId = _testData.OriginatorIdDictionary[$"{nameof(_testData.EditorExistEvent2)}"];

            _service.IsPasswordRequired(noRelationId, ev).IsTrue();
            _service.IsPasswordRequired(inputUserId, ev).IsFalse();
            _service.IsPasswordRequired(memberUserId, ev).IsFalse();
            _service.IsPasswordRequired(editorId, ev).IsFalse();
            _service.IsPasswordRequired(ownerId, ev).IsFalse();
        }

        [Test]
        public void Remove()
        {
            _events.DeleteMany(ev => true);
        }

        [TearDown]
        public void TearDown()
        {
            _events.DeleteMany(ev => true);
        }
    }
}