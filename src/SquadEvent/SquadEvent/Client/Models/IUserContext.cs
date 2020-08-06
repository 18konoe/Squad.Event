using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace SquadEvent.Client.Models
{
    public interface IUserContext
    {
        IIdentity UserIdentity { get; set; }
        IEnumerable<Claim> UserClaims { get; set; }
    }
}