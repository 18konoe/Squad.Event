using System;
using System.Collections.Generic;
using MongoDB.Driver;
using SquadEvent.Server.Model;
using SquadEvent.Shared.Models;
using SquadEvent.Shared.Parameters;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Server.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<EventModel> _events;

        public EventService(IMongoCollectionProvider<EventModel> provider)
        {
            _events = provider.GetCollection();
        }

        #region Get Events

        public EventModel GetEventById(string id)
        {
            try
            {
                return GetEventFluentById(id).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IList<EventModel> GetEventsByOriginatorId(string userId) => GetEventFluentByOriginatorId(userId).ToList();
        public IList<EventModel> GetEventsByEditorId(string userId) => GetEventsFluentByEditorId(userId).ToList();
        public IList<EventModel> GetEventsByInputUserId(string userId) => GetEventsFluentByInputUserId(userId).ToList();

        public IEventModel GetEventUpdateByParam(EventIdentifyParameter param)
        {
            try
            {
                return GetEventUpdateFluentByParam(param).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEventModel GetEventDeleteByParam(EventIdentifyParameter param)
        {
            try
            {
                return GetEventDeleteFluentByParam(param).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEventModel GetEventScheduleUpdateByParam(EventIdentifyParameter param)
        {
            try
            {
                return GetScheduleUpdateFluentByParam(param).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Get IFindFluents

        public IFindFluent<EventModel, EventModel> GetEventFluentById(string id) => _events.Find(GetEventByIdFilterDefinition(id));

        public IFindFluent<EventModel, EventModel> GetEventFluentByOriginatorId(string userId) =>
            _events.Find(GetEventByOriginatorIdDefinition(userId));
        public IFindFluent<EventModel, EventModel> GetEventsFluentByEditorId(string userId) => _events.Find(
            Builders<EventModel>.Filter.And(
                GetEventByStatesFilterDefinition(EventState.Draft, EventState.Open, EventState.Fixed),
                GetEventByEditorIdFilterDefinition(userId)));
        public IFindFluent<EventModel, EventModel> GetEventsFluentByInputUserId(string userId) => _events.Find(
            Builders<EventModel>.Filter.And(
                GetEventByStatesFilterDefinition(EventState.Open, EventState.Fixed),
                GetEventByInputUserIdFilterDefinition(userId)));
        public IFindFluent<EventModel, EventModel> GetEventUpdateFluentByParam(EventIdentifyParameter param) =>
            _events.Find(GetEventUpdateFilterDefinition(param));
        public IFindFluent<EventModel, EventModel> GetEventDeleteFluentByParam(EventIdentifyParameter param) =>
            _events.Find(GetEventDeleteFilterDefinition(param));
        public IFindFluent<EventModel, EventModel> GetScheduleUpdateFluentByParam(EventIdentifyParameter param) =>
            _events.Find(GetScheduleUpdateFilterDefinition(param));

        #endregion

        #region Get FilterDefinitions

        public FilterDefinition<EventModel> GetEventByIdFilterDefinition(string eventId) =>
            Builders<EventModel>.Filter.Eq(ev => ev.Id, eventId);
        public FilterDefinition<EventModel> GetEventByPasswordFilterDefinition(string hashedPassword) =>
            Builders<EventModel>.Filter.Eq(ev => ev.HashedPassword, hashedPassword);
        
        public FilterDefinition<EventModel> GetEventUpdateFilterDefinition(EventIdentifyParameter param) =>
            Builders<EventModel>.Filter.And(
                GetEventByIdFilterDefinition(param.Id),
                GetEventByPasswordFilterDefinition(param.Phrase),
                Builders<EventModel>.Filter.Or(
                    GetEventByOriginatorIdDefinition(param.UserId),
                    GetEventByEditorIdFilterDefinition(param.UserId)
                    ));

        public FilterDefinition<EventModel> GetEventDeleteFilterDefinition(EventIdentifyParameter param) =>
            Builders<EventModel>.Filter.And(
                GetEventByIdFilterDefinition(param.Id),
                GetEventByPasswordFilterDefinition(param.Phrase),
                GetEventByOriginatorIdDefinition(param.UserId)
                );

        public FilterDefinition<EventModel> GetScheduleUpdateFilterDefinition(EventIdentifyParameter param) =>
            Builders<EventModel>.Filter.And(
                GetEventByIdFilterDefinition(param.Id),
                GetEventByPasswordFilterDefinition(param.Phrase));

        public FilterDefinition<EventModel> GetEventByOriginatorIdDefinition(string userId) =>
            Builders<EventModel>.Filter.Eq(ev => ev.Originator, userId);
        public FilterDefinition<EventModel> GetEventByStatesFilterDefinition(params EventState[] states) =>
            Builders<EventModel>.Filter.In(ev => ev.State, states);

        public FilterDefinition<EventModel> GetEventByEditorIdFilterDefinition(string userId) =>
            Builders<EventModel>.Filter.AnyEq(ev => ev.Editors, userId);

        public FilterDefinition<EventModel> GetEventByInputUserIdFilterDefinition(string userId) =>
            Builders<EventModel>.Filter.Exists(GetScheduleValueFieldDefinition(userId));

        #endregion

        #region Get FieldDefinitions

        public StringFieldDefinition<EventModel, Schedule> GetScheduleValueFieldDefinition(string userId) =>
            new StringFieldDefinition<EventModel, Schedule>($"{nameof(EventModel.Schedules)}.{userId}");

        #endregion

        #region AddOrUpdate
        
        public EventModel AddEvent(EventModel ev)
        {
            try
            {
                _events.InsertOne(ev);
            }
            catch (Exception)
            {
                return null;
            }

            return ev;
        }

        public EventModel UpdateEvent(EventIdentifyParameter param, EventModel ev)
        {
            return _events.FindOneAndUpdate(GetEventUpdateFilterDefinition(param), Builders<EventModel>.Update
                .Set(e => e.Name, ev.Name)
                .Set(e => e.Description, ev.Description)
                .Set(e => e.Dates, ev.Dates)
                .Set(e => e.Permission, ev.Permission)
                .Set(e => e.LastUpdated, DateTimeOffset.Now)
                .Set(e => e.Editors, ev.Editors)
                .Set(e => e.Members, ev.Members)
                .Set(e => e.Guild, ev.Guild)
                .Set(e => e.Channel, ev.Channel)
                .Set(e => e.MinEntry, ev.MinEntry)
                .Set(e => e.MaxEntry, ev.MaxEntry)
                .Set(e => e.HashedPassword, ev.HashedPassword)
                .Set(e => e.State, ev.State)
                .Set(e => e.FixDate, ev.FixDate)
            );
        }

        public EventModel UpdateEventState(EventIdentifyParameter param, EventState state, DateTimeOffset? fixDate = null)
        {
            if (state == EventState.Draft || param == null) return null;
            var baseUpdateQuery = Builders<EventModel>.Update.Set(ev => ev.State, state)
                .Set(e => e.LastUpdated, DateTimeOffset.Now);
            if (state != EventState.Fixed && state != EventState.Closed)
            {
                baseUpdateQuery = baseUpdateQuery.Set(ev => ev.FixDate, null);
            }
            else if (state == EventState.Fixed)
            {
                if (fixDate == null)
                {
                    throw new ArgumentNullException($"nameof{fixDate} must not null if state is Fixed.");
                }
                baseUpdateQuery = baseUpdateQuery.Set(ev => ev.FixDate, fixDate);
            }
            return _events.FindOneAndUpdate(GetEventUpdateFilterDefinition(param), baseUpdateQuery);
        }

        public EventModel AddOrUpdateSchedule(EventIdentifyParameter param, Schedule schedule)
        {
            return _events.FindOneAndUpdate(GetScheduleUpdateFilterDefinition(param),
                Builders<EventModel>.Update.Set(GetScheduleValueFieldDefinition(param.UserId), schedule));
        }

        #endregion

        #region Delete

        public EventModel DeleteEvent(EventIdentifyParameter param)
        {
            return _events.FindOneAndDelete(GetEventDeleteFilterDefinition(param));
        }

        #endregion

        #region Others

        public EventModel FilterSchedule(string userId, EventModel ev)
        {
            bool permit = false;
            switch (ev.Permission)
            {
                case SchedulePermissionToKnow.All:
                    permit = true;
                    break;
                case SchedulePermissionToKnow.Input:
                    permit = ev.Originator == userId || ev.Editors.Contains(userId) || ev.Members.Contains(userId) || ev.Schedules.Keys.Contains(userId);
                    break;
                case SchedulePermissionToKnow.Member:
                    permit = ev.Originator == userId || ev.Editors.Contains(userId) || ev.Members.Contains(userId);
                    break;
                case SchedulePermissionToKnow.Editor:
                    permit = ev.Originator == userId || ev.Editors.Contains(userId);
                    break;
                case SchedulePermissionToKnow.Originator:
                    permit = ev.Originator == userId;
                    break;
            }
            if (permit) return ev;

            var hasInput = ev.Schedules.TryGetValue(userId, out Schedule userSchedule);
            ev.Schedules.Clear();
            if (hasInput && userSchedule.UserId == userId)
            {
                ev.Schedules.Add(userId, userSchedule);
            }

            return ev;
        }

        public bool IsPasswordRequired(string userId, EventModel ev)
        {
            if (string.IsNullOrEmpty(ev.HashedPassword)) return false;
            return !(ev.Originator.IsEqualIgnoreCase(userId) || ev.Editors.Contains(userId) || ev.Members.Contains(userId) ||
                     ev.Schedules.ContainsKey(userId));
        }

        #endregion
    }
}