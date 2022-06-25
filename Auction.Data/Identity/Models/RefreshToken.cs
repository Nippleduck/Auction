using System;

namespace Auction.Data.Identity.Models
{
    public class RefreshToken
    {
        public Guid Token { get; set; }
        public string JwtId { get; set; }
        public string UserId { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}
