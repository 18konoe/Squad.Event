using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Shared.Models
{
    public interface IEventModel : IValueEquatable<IEventModel>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        //[BsonIdHexStrings]
        string Id { get; set; }
        [BsonRequired]
        [StringLength(256)]
        string Name { get; set; }
        [BsonRequired]
        ICollection<DateTimeOffset> Dates { get; set; }

        [StringLength(2048)]
        string Description { get; set; }
        [BsonRequired]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        EventState State { get; set; }
        [BsonRequired]
        string Originator { get; set; }
        [BsonRequired]
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        IDictionary<string, Schedule> Schedules { get; set; }
        [BsonRequired]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        SchedulePermissionToKnow Permission { get; set; }
        [BsonRequired]
        DateTimeOffset LastUpdated { get; set; }
        ICollection<string> Editors { get; set; }
        ICollection<string> Members { get; set; }
        string Guild { get; set; }
        string Channel { get; set; }
        int? MinEntry { get; set; }
        int? MaxEntry { get; set; }
        DateTimeOffset? FixDate { get; set; }
        string HashedPassword { get; set; }
    }
}