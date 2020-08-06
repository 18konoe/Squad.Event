using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Shared.Models
{
    public class EventModel : IEventModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        //[BsonIdHexStrings]
        public string Id { get; set; }
        [BsonRequired]
        [StringLength(256)]
        public string Name { get; set; } = string.Empty;
        [BsonRequired]
        public ICollection<DateTimeOffset> Dates { get; set; } = new List<DateTimeOffset>();

        [StringLength(2048)]
        public string Description { get; set; } = string.Empty;
        [BsonRequired]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public EventState State { get; set; } = EventState.Draft;
        [BsonRequired]
        public string Originator { get; set; } = string.Empty;
        [BsonRequired]
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public IDictionary<string, Schedule> Schedules { get; set; } = new Dictionary<string, Schedule>();
        [BsonRequired]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public SchedulePermissionToKnow Permission { get; set; } = SchedulePermissionToKnow.All;
        [BsonRequired]
        public DateTimeOffset LastUpdated { get; set; } = DateTime.Now;
        public ICollection<string> Editors { get; set; } = new List<string>();
        public ICollection<string> Members { get; set; } = new List<string>();
        public string Guild { get; set; }
        public string Channel { get; set; }
        public int? MinEntry { get; set; }
        public int? MaxEntry { get; set; }
        public DateTimeOffset? FixDate { get; set; }
        public string HashedPassword { get; set; }
        
        public bool ValueEquals(IEventModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Id != other.Id) return false;
            if (!LastUpdated.Equals(other.LastUpdated)) return false;
            if (Originator != other.Originator) return false;
            if (Name != other.Name) return false;
            if (Description != other.Description) return false;
            if (State != other.State) return false;
            if (Permission != other.Permission) return false;
            if (Channel != other.Channel) return false;
            if (Guild != other.Guild) return false;
            if (MinEntry != other.MinEntry) return false;
            if (MaxEntry != other.MaxEntry) return false;
            if (!Nullable.Equals(FixDate, other.FixDate)) return false;
            if (HashedPassword != other.HashedPassword) return false;
            if (!Editors.ElementalEquals(other.Editors)) return false;
            if (!Members.ElementalEquals(other.Members)) return false;
            if (!Dates.ElementalEquals(other.Dates)) return false;
            if (!Schedules.ValueEquals(other.Schedules)) return false;
            return true;
        }
    }

    public enum EventState
    {
        Draft,
        Open,
        Fixed,
        Closed
    }

    public enum SchedulePermissionToKnow
    {
        Originator,
        Editor,
        Member,
        Input,
        All
    }
}