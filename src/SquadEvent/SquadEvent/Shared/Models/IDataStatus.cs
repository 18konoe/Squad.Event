using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Shared.Models
{
    public interface IDataStatus : IValueEquatable<IDataStatus>
    {
        [BsonRequired]
        DateTimeOffset Date { get; set; }
        [BsonRequired]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        AttendanceStatus Status { get; set; }
    }
}