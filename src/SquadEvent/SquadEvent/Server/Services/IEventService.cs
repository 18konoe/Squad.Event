using System;
using SquadEvent.Shared.Models;
using SquadEvent.Shared.Parameters;

namespace SquadEvent.Server.Services
{
    public interface IEventService
    {
        EventModel AddEvent(EventModel ev);
        EventModel GetEventById(string id);
        EventModel FilterSchedule(string userId, EventModel ev);
        EventModel UpdateEvent(EventIdentifyParameter param, EventModel ev);
        EventModel DeleteEvent(EventIdentifyParameter param);
        EventModel AddOrUpdateSchedule(EventIdentifyParameter param, Schedule schedule);
        bool IsPasswordRequired(string userId, EventModel ev);
    }
}