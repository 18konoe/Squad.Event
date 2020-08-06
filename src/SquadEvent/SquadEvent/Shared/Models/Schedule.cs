using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Shared.Models
{
    public class Schedule : ISchedule
    {
        [BsonRequired]
        public string UserId { get; set; } = string.Empty;
        [BsonRequired]
        public string Name { get; set; } = string.Empty;
        [BsonRequired]
        public IList<DateStatus> DateStatuses { get; set; } = new List<DateStatus>();

        [StringLength(64)]
        public string Comment { get; set; } = string.Empty;

        public bool ValueEquals(ISchedule other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (UserId != other.UserId) return false;
            if (Name != other.Name) return false;
            if (Comment != other.Comment) return false;
            if (!DateStatuses.ValueEquals(other.DateStatuses)) return false;
            return true;
        }
    }
}