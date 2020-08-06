using System.ComponentModel.DataAnnotations;
using SquadEvent.Shared.Validations;

namespace SquadEvent.Shared.Parameters
{
    public class BsonIdParameter
    {
        [Required]
        [BsonIdHexStrings]
        public string Id { get; set; }
    }
}