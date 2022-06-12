using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Auction.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Auction.Domain.Entities;

namespace Auction.Data.Context
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions options) : base(options) { }

        public DbSet<Lot> Lots { get; set; }
        public DbSet<BiddingDetails> BiddingDetails { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
