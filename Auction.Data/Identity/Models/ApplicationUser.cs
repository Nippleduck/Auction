using Auction.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auction.Data.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Person Person { get; set; }
        public int PersonId { get; set; }
    }
}
