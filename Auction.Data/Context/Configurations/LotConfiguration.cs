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
                .HasForeignKey(l => l.SellerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(l => l.Category)
                .WithMany(c => c.Lots);

            builder
                .HasOne(l => l.Status)
                .WithMany(s => s.Lots);

            builder
                .Property(l => l.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(lot => lot.StartPrice)
                .HasColumnType("decimal(10,2)")
                .IsRequired();
        }
    }
}
