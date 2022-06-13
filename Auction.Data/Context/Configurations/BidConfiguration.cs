using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Data.Context.Configurations
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder
                .HasOne(b => b.Bidder)
                .WithMany(p => p.Bids)
                .HasForeignKey(b => b.BidderId);

            builder
                .Property(b => b.Price)
                .IsRequired();

            builder
                .Property(b => b.PlacedOn)
                .IsRequired();
        }
    }
}
