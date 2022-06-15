using Auction.Data.Identity;
using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Data.Context.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasOne<ApplicationUser>()
                .WithOne(u => u.Person)
                .HasForeignKey<ApplicationUser>(u => u.PersonId);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

            builder.Property(p => p.Surname).IsRequired().HasMaxLength(100);

            builder.Property(P => P.BirthDate).IsRequired();
        }
    }
}
