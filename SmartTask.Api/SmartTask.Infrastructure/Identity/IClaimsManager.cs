using SmartTask.Domain.AggregatesModel.UserAggregate;
using System.Collections.Generic;
using System.Security.Claims;

namespace SmartTask.Infrastructure.Identity
{
    public interface IClaimsManager
    {
        int GetCurrentUserId();

        string GetCurrentUserName();

        IEnumerable<Claim> GetUserClaims(User user);

        Claim GetUserClaim(string claimType);
    }
}
