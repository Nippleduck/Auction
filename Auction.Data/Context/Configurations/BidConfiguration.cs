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

            builder.HasOne(b => b.BiddingDetails)
                .WithMany(bd => bd.Bids)
                .HasForeignKey(b => b.BiddingDetailsId);

            builder
                .Property(b => b.Price)
                .HasColumnType("decimal(10, 2)")
                .IsRequired();

            builder
                .Property(b => b.PlacedOn)
                .IsRequired();
        }
    }
}
