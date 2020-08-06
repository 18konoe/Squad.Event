using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace SquadEvent.Client
{
    public interface IUserContext
    {
        IIdentity UserIdentity { get; set; }
        IEnumerable<Claim> UserClaims { get; set; }
    }
}