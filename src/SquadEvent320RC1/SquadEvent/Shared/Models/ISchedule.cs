using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Shared.Models
{
    public interface ISchedule : IValueEquatable<ISchedule>
    {
        [BsonRequired]
        string UserId { get; set; }
        [BsonRequired]
        string Name { get; set; }
        [BsonRequired]
        IList<DateStatus> DateStatuses { get; set; }

        [StringLength(64)]
        string Comment { get; set; }
    }
}