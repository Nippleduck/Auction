﻿using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Auction.Data.Context.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

            builder.Property(p => p.Surname).IsRequired().HasMaxLength(100);

            builder.Property(P => P.BirthDate).IsRequired();
        }
    }
}