using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SquadEvent.Server.Services;
using SquadEvent.Shared.Models;
using SquadEvent.Shared.Parameters;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Server.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService service)
        {
            _eventService = service;
        }

        [HttpGet("/event/{id}")]
        

        [HttpPost("/api/Create")]
        public ActionResult<EventIdResponse> CreateEvent([FromBody]EventModel ev)
        {
            var userId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest($"User Id is null");
            if (userId != ev.Originator) return BadRequest($"Wrong User Id: {userId}");

            if (ev.State == EventState.Fixed || ev.State == EventState.Closed) return BadRequest($"Could not create event with {nameof(ev.State)} = {EventState.Fixed} or {EventState.Closed}");
            if (ev.State == EventState.Open)
            {
                if (ev.Dates.Count == 0) return BadRequest($"{EventState.Open} require something {nameof(ev.Dates)} data");
                if (string.IsNullOrEmpty(ev.Name) || string.IsNullOrWhiteSpace(ev.Name)) return BadRequest($"{EventState.Open} require something {nameof(ev.Name)}");
            }

            if(ev.FixDate != null) return BadRequest($"{EventState.Draft} or {EventState.Open} needs {nameof(ev.FixDate)} empty");
            if(ev.MinEntry != null && ev.MaxEntry != null && ev.MinEntry.Value > ev.MaxEntry.Value) return BadRequest($"{nameof(ev.MinEntry)} needs lower than {nameof(ev.MaxEntry)}");
            if (ev.Schedules.Count != 0) return BadRequest($"{nameof(ev.Schedules)} needs empty");
            
            var result = _eventService.AddEvent(ev);

            if (result == null) return BadRequest("Failed to insert event.");
            return new EventIdResponse(result.Id);
        }

        [HttpPost("/api/Event/{id}")]
        public ActionResult<EventModel> GetEvent(string id)
        {
            var userId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest($"User Id is null");

            var ev = _eventService.GetEventById(id);
            if (ev == null) return BadRequest($"Not exist Id: {id}");

            if (_eventService.IsPasswordRequired(userId, ev)) return Unauthorized();

            return _eventService.FilterSchedule(userId, ev);
        }

        [HttpPost("/api/Event")]
        public ActionResult<EventModel> GetEventWithPassword([FromBody] EventIdentifyParameter param)
        {
            var userId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest($"User Id is null");
            if (userId != param.UserId) return BadRequest($"Wrong User Id: {param.UserId}");

            var ev = _eventService.GetEventById(param.Id);
            if (ev == null) return BadRequest($"Not exist Id: {param.Id}");

            if (!_eventService.IsPasswordRequired(userId, ev)) return _eventService.FilterSchedule(userId, ev);

            if (!ev.HashedPassword.IsEqualIgnoreCase(param.Phrase)) return Unauthorized();
            
            return _eventService.FilterSchedule(userId, ev);
        }

        [HttpPost("/api/EditEvent")]
        public IActionResult UpdateEvent([FromBody] EventIdentifyParameter param)
        {
            if(param.AttachedEvent == null) return BadRequest($"{nameof(param.AttachedEvent)} is null");
            var userId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest($"User Id is null");
            if (userId != param.UserId) return BadRequest($"Wrong User Id: {param.UserId}");

            if (param.AttachedEvent.MinEntry != null && param.AttachedEvent.MaxEntry != null && param.AttachedEvent.MinEntry.Value > param.AttachedEvent.MaxEntry.Value) return BadRequest($"{nameof(param.AttachedEvent.MinEntry)} needs lower than {nameof(param.AttachedEvent.MaxEntry)}.");

            switch (param.AttachedEvent.State)
            {
                case EventState.Draft:
                    return BadRequest($"Could not revert to {EventState.Draft} state.");
                case EventState.Open:
                    if (param.AttachedEvent.Dates.Count == 0) return BadRequest($"{EventState.Open} require something {nameof(param.AttachedEvent.Dates)} data.");
                    if (string.IsNullOrEmpty(param.AttachedEvent.Name) || string.IsNullOrWhiteSpace(param.AttachedEvent.Name)) return BadRequest($"{EventState.Open} require something {nameof(param.AttachedEvent.Name)}.");
                    break;
                case EventState.Fixed:
                    if (param.AttachedEvent.Dates.Count == 0) return BadRequest($"{EventState.Fixed} require something {nameof(param.AttachedEvent.Dates)} data.");
                    if (string.IsNullOrEmpty(param.AttachedEvent.Name) || string.IsNullOrWhiteSpace(param.AttachedEvent.Name)) return BadRequest($"{EventState.Open} require something {nameof(param.AttachedEvent.Name)}.");
                    if (param.AttachedEvent.Schedules.Count == 0) return BadRequest($"{nameof(param.AttachedEvent.Schedules)} required at least 1");
                    if (param.AttachedEvent.FixDate == null) return BadRequest($"{EventState.Fixed} require to set {nameof(param.AttachedEvent.FixDate)}.");
                    break;
                case EventState.Closed:
                    if (param.AttachedEvent.FixDate == null) return BadRequest($"{EventState.Closed} require to set {nameof(param.AttachedEvent.FixDate)}.");
                    if (param.AttachedEvent.FixDate > DateTimeOffset.Now) return BadRequest($"{EventState.Closed} require to set {nameof(param.AttachedEvent.FixDate)} is past time.");
                    break;
            }

            var updatedEvent = _eventService.UpdateEvent(param, param.AttachedEvent);
            
            if(updatedEvent == null) return BadRequest($"Updated event is nothing.");
            return Ok();
        }

        [HttpDelete("/api/Event")]
        public IActionResult DeleteEvent([FromBody] EventIdentifyParameter param)
        {
            var userId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest($"User Id is null");
            if (userId != param.UserId) return BadRequest($"Wrong User Id: {param.UserId}");

            var removedEvent = _eventService.DeleteEvent(param);

            if (removedEvent == null) return BadRequest($"Not exist Id: {param.Id} and Hash: {param.Phrase}");
            return Ok();
        }

        [HttpPost("/api/Schedule")]
        public IActionResult AddOrUpdateSchedule([FromBody] EventIdentifyParameter param)
        {
            if(param.AttachedSchedule == null) return BadRequest($"{nameof(param.AttachedSchedule)} is null");
            var userId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest($"User Id is null");
            if (userId != param.UserId) return BadRequest($"Wrong User Id: {param.UserId}");
            if (userId != param.AttachedSchedule.UserId) return BadRequest($"Wrong User Id: {param.AttachedSchedule.UserId}");

            var updatedEvent = _eventService.AddOrUpdateSchedule(param, param.AttachedSchedule);
            if(updatedEvent == null) return BadRequest($"Updated event is nothing.");

            return Ok();
        }
    }
}