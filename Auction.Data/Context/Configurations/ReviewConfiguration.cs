using Auction.Domain.Entities;
using Auction.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Data.Context.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewDetails>
    {
        public void Configure(EntityTypeBuilder<ReviewDetails> builder)
        {
            builder.Property(r => r.Status)
                .HasDefaultValue(ReviewStatus.PendingReview);
        }
    }
}
