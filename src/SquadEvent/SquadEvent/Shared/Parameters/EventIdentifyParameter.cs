using System.ComponentModel.DataAnnotations;
using SquadEvent.Shared.Models;
using SquadEvent.Shared.Validations;

namespace SquadEvent.Shared.Parameters
{
    public class EventIdentifyParameter
    {
        [Required]
        [BsonIdHexStrings]
        public string Id { get; set; }
        [Required]
        [Sha256HashStrings]
        public string Phrase { get; set; }
        [Required]
        [LongNumberId]
        public string UserId { get; set; }
        public EventModel AttachedEvent { get; set; }
        public Schedule AttachedSchedule { get; set; }
    }
}