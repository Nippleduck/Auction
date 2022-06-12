using Microsoft.AspNetCore.Identity;

namespace Auction.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int PersonId { get; set; }
    }
}
