using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace SquadEvent.Client.Models
{
    public class UserContext : IUserContext
    {
        public IIdentity UserIdentity { get; set; }
        public IEnumerable<Claim> UserClaims { get; set; }
        public Uri UserImageUri { get; set; }
    }
}