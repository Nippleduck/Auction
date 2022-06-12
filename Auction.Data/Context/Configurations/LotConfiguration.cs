using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Data.Context.Configurations
{
    internal class LotConfiguration : IEntityTypeConfiguration<Lot>
    {
        public void Configure(EntityTypeBuilder<Lot> builder)
        {
            builder
                .HasOne(l => l.BiddingDetails)
                .WithOne(bd => bd.Lot)
                .HasForeignKey<BiddingDetails>(bd => bd.LotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(l => l.ReviewDetails)
                .WithOne(rd => rd.Lot)
                .HasForeignKey<ReviewDetails>(rd => rd.LotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(l => l.Seller)
                .WithMany(p => p.OwnedLots)
                .IsRequired();

            builder.
                HasOne(l => l.Buyer)
                .WithMany(p => p.PurchasedLots);

            builder
                .HasOne(l => l.Category)
                .WithMany();

            builder
                .HasOne(l => l.Status)
                .WithMany();

            builder
                .Property(l => l.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
