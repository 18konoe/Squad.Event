using System.ComponentModel.DataAnnotations;
using SquadEvent.Shared.Validations;

namespace SquadEvent.Shared.Parameters
{
    public class EventIdResponse
    {
        [Required]
        [BsonIdHexStrings]
        public string Id { get; set; }

        public EventIdResponse(string id)
        {
            Id = id;
        }

        public EventIdResponse()
        {

        }
    }
}