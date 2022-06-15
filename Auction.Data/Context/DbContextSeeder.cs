using Auction.Data.Identity;
using Auction.Domain.Entities;
using Auction.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Data.Context
{
    public class DbContextSeeder
    {
        public DbContextSeeder(AuctionContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        private readonly AuctionContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public async Task TrySeedAsync()
        {
            var customerRole = new IdentityRole("Customer");
            
            if (!roleManager.Roles.Any(r => r.Name == customerRole.Name))
            {
                await roleManager.CreateAsync(customerRole);
            }

            var adminRole = new IdentityRole("Administrator");

            if (!roleManager.Roles.Any(r => r.Name == adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }

            var admin = new ApplicationUser { Email = "admin@admin" };

            if (!userManager.Users.Any(u => u.Email == admin.Email))
            {
                await userManager.CreateAsync(admin, "admin");
                await userManager.AddToRoleAsync(admin, adminRole.Name);
            }

            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(new AuctionStatus[]
                {
                    new AuctionStatus { Id = 1, Name = "Auctioning" },
                    new AuctionStatus { Id = 2, Name = "Lot Sold" },
                    new AuctionStatus { Id = 3, Name = "Over" },
                    new AuctionStatus { Id = 4, Name = "Not Started" }
                });
            }

            if (!context.Categories.Any())
            {
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
            }

            if (!context.Persons.Any())
            {
                context.Persons.AddRange(new Person[]
                {
                    new Person { Id = 1, Name = "James", Surname = "Longhart", BirthDate = new DateTime(1997, 4, 3)},
                    new Person { Id = 2, Name = "Sara", Surname = "Parker", BirthDate = new DateTime(1972, 11, 2)},
                    new Person { Id = 3, Name = "Hanna", Surname = "Winslow", BirthDate = new DateTime(1990, 1, 1)}
                });
            }

            if (!context.Lots.Any())
            {
                context.Lots.AddRange(new Lot[]
                {
                    new Lot { Id = 1, Name = "Emerald ring", CategoryId = 2, SellerId = 1, StartPrice = 1200, StatusId = 1, OpenDate = DateTime.UtcNow, CloseDate = DateTime.UtcNow.AddDays(10) },
                    new Lot { Id = 2, Name = "Landscape painting", CategoryId = 3, SellerId = 1, BuyerId = 3, StartPrice = 500, StatusId = 2, OpenDate = new DateTime(2021, 1, 11), CloseDate = new DateTime(2021, 3, 11) },
                    new Lot { Id = 3, Name = "Leather Jacket", CategoryId = 1, SellerId = 2, StartPrice = 200, StatusId = 3, OpenDate = new DateTime(2022, 1, 1), CloseDate = new DateTime(2022, 1, 11)},
                    new Lot { Id = 4, Name = "War and Peace First Edition", CategoryId = 7, SellerId = 2, StatusId = 4, StartPrice = 350}
                });
            }

            if (!context.BiddingDetails.Any())
            {
                context.BiddingDetails.AddRange(new BiddingDetails[]
                {
                    new BiddingDetails { Id = 1, LotId = 1, MinimalBid = 200, CurrentBid = 1200 },
                    new BiddingDetails { Id = 2, LotId = 2, MinimalBid = 100, CurrentBid = 1300, Sold = true },
                    new BiddingDetails { Id = 3, LotId = 3, MinimalBid = 50, CurrentBid = 200 }
                });
            }

            if (!context.Bids.Any())
            {
                context.Bids.AddRange(new Bid[]
                {
                    new Bid { Id = 1, BiddingDetailsId = 2, BidderId = 2, PlacedOn = new DateTime(2021, 1, 2), Price = 400 },
                    new Bid { Id = 2, BiddingDetailsId = 2, BidderId = 3, PlacedOn = new DateTime(2021, 1, 3), Price = 400 }
                });
            }

            if (!context.Reviews.Any())
            {
                context.Reviews.AddRange(new ReviewDetails[]
                {
                    new ReviewDetails { Id = 1, LotId = 1, Status = ReviewStatus.Allowed },
                    new ReviewDetails { Id = 2, LotId = 2, Status = ReviewStatus.Allowed },
                    new ReviewDetails { Id = 3, LotId = 3, Status = ReviewStatus.Allowed },
                    new ReviewDetails { Id = 4, LotId = 4, Status = ReviewStatus.PendingReview}
                });
            }
        }
    }
}
