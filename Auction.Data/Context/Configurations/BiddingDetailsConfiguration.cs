using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Data.Context.Configurations
{
    internal class BiddingDetailsConfiguration : IEntityTypeConfiguration<BiddingDetails>
    {
        public void Configure(EntityTypeBuilder<BiddingDetails> builder)
        {
            builder
                .HasOne(bd => bd.Buyer)
                .WithMany()
                .HasForeignKey(bd => bd.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(bd => bd.CurrentBid)
                .HasColumnType("decimal(10, 2)");

            builder
                .Property(bd => bd.MinimalBid)
                .HasColumnType("decimal(10, 2)");
        }
    }
}
