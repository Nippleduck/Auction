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
                .Property(bd => bd.CurrentBid)
                .HasColumnType("decimal(10, 2)");
        }
    }
}
