using Auction.Data.Context;
using Auction.Data.Identity;
using Auction.Domain.Entities;
using Auction.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Auction.Data.Tests
{
    internal static class TestDbContextProvider
    {
        public static AuctionContext CreateContext() => new AuctionContext(GetDbOptions());

        private static DbContextOptions<AuctionContext> GetDbOptions()
        {
            var options = new DbContextOptionsBuilder<AuctionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AuctionContext(options);

            SeedContext(context);

            return options;
        }

        private static void SeedContext(AuctionContext context)
        {
            context.Statuses.AddRange(new AuctionStatus[]
                {
                    new AuctionStatus { Id = 1, Name = "Auctioning" },
                    new AuctionStatus { Id = 2, Name = "Lot Sold" },
                    new AuctionStatus { Id = 3, Name = "Over" },
                    new AuctionStatus { Id = 4, Name = "Not Started" }
                });

            context.Categories.AddRange(new Category[]
               {
                    new Category { Id = 1, Name = "Clothes" },
                    new Category { Id = 2, Name = "Jewelry" },
                    new Category { Id = 3, Name = "Art" },
                    new Category { Id = 4, Name = "Games" },
                    new Category { Id = 5, Name = "Sculpture" },
                    new Category { Id = 6, Name = "Misc"},
                    new Category { Id = 7, Name = "Books"}
               });

            context.Persons.AddRange(new Person[]
                {
                    new Person { Id = 1, Name = "James", Surname = "Longhart", BirthDate = new DateTime(1997, 4, 3)},
                    new Person { Id = 2, Name = "Sara", Surname = "Parker", BirthDate = new DateTime(1972, 11, 2)},
                    new Person { Id = 3, Name = "Hanna", Surname = "Winslow", BirthDate = new DateTime(1990, 1, 1)}
                });

            context.Lots.AddRange(new Lot[]
                {
                    new Lot { Id = 1, Name = "Emerald ring", CategoryId = 2, SellerId = 1, StartPrice = 1200, StatusId = 1, OpenDate = new DateTime(2022, 6, 16), CloseDate = new DateTime(2022, 6, 26) },
                    new Lot { Id = 2, Name = "Landscape painting", CategoryId = 3, SellerId = 1, BuyerId = 3, StartPrice = 500, StatusId = 2, OpenDate = new DateTime(2021, 1, 11), CloseDate = new DateTime(2021, 3, 11) },
                    new Lot { Id = 3, Name = "Leather Jacket", CategoryId = 1, SellerId = 2, StartPrice = 200, StatusId = 3, OpenDate = new DateTime(2022, 1, 1), CloseDate = new DateTime(2022, 1, 11)},
                    new Lot { Id = 4, Name = "War and Peace First Edition", CategoryId = 7, SellerId = 2, StatusId = 4, StartPrice = 350}
                });

            context.BiddingDetails.AddRange(new BiddingDetails[]
                {
                    new BiddingDetails { Id = 1, LotId = 1, MinimalBid = 200, CurrentBid = 1200 },
                    new BiddingDetails { Id = 2, LotId = 2, MinimalBid = 100, CurrentBid = 1300, Sold = true },
                    new BiddingDetails { Id = 3, LotId = 3, MinimalBid = 50, CurrentBid = 200 }
                });

            context.Bids.AddRange(new Bid[]
                {
                    new Bid { Id = 1, BiddingDetailsId = 2, BidderId = 2, PlacedOn = new DateTime(2021, 1, 2), Price = 400 },
                    new Bid { Id = 2, BiddingDetailsId = 2, BidderId = 3, PlacedOn = new DateTime(2021, 1, 3), Price = 400 }
                });

            context.Reviews.AddRange(new ReviewDetails[]
                {
                    new ReviewDetails { Id = 1, LotId = 1, Status = ReviewStatus.Allowed },
                    new ReviewDetails { Id = 2, LotId = 2, Status = ReviewStatus.Allowed },
                    new ReviewDetails { Id = 3, LotId = 3, Status = ReviewStatus.Allowed },
                    new ReviewDetails { Id = 4, LotId = 4, Status = ReviewStatus.PendingReview}
                });

            context.SaveChanges();
        }
    }
}
