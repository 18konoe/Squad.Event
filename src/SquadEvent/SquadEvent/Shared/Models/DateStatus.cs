using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace SquadEvent.Shared.Models
{
    public class DateStatus : IDataStatus
    {
        public DateStatus(DateTimeOffset date, AttendanceStatus status)
        {
            Date = date;
            Status = status;
        }
        [BsonRequired]
        public DateTimeOffset Date { get; set; }
        [BsonRequired]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public AttendanceStatus Status { get; set; }

        public bool ValueEquals(IDataStatus other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Date.Equals(other.Date) && Status == other.Status;
        }
    }

    public enum AttendanceStatus
    {
        Present,
        Absent,
        Difficult,
        Hope
    }
}