using Microsoft.AspNetCore.Identity;

namespace Auction.Data.Identity
{
    internal class UserCredentials : IdentityUser
    {
        public int PersonId { get; set; }
    }
}
