using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Auction.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Auction.Domain.Entities;
using System.Reflection;

namespace Auction.Data.Context
{
    public class AuctionContext : IdentityDbContext<ApplicationUser>
    {
        public AuctionContext() { }
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }

        public DbSet<Lot> Lots { get; set; }
        public DbSet<ReviewDetails> Reviews { get; set; }
        public DbSet<BiddingDetails> BiddingDetails { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<AuctionStatus> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
