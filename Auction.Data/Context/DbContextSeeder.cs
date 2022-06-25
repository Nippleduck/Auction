using Auction.Data.Identity.Models;
using Auction.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

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

        public async Task InitializeAsync()
        {
            try
            {
                if (context.Database.IsSqlServer())
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception) { throw; }
        }

        public async Task TrySeedAsync()
        {
            var customerRole = new IdentityRole(Roles.Customer.ToString());
            
            if (!roleManager.Roles.Any(r => r.Name == customerRole.Name))
            {
                await roleManager.CreateAsync(customerRole);
            }

            var adminRole = new IdentityRole(Roles.Administrator.ToString());

            if (!roleManager.Roles.Any(r => r.Name == adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }

            var admin = new ApplicationUser 
            { 
                Email = "admin@admin",
                UserName = "admin",
                Person = new Person {
                    Name = "James",
                    Surname = "Farlow",
                    BirthDate = DateTime.UtcNow
                }
            };

            if (!userManager.Users.Any(u => u.Email == admin.Email))
            {
                await userManager.CreateAsync(admin, "Adminadmin123!");
                await userManager.AddToRoleAsync(admin, adminRole.Name);
            }

            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(new AuctionStatus[]
                {
                    new AuctionStatus { Name = "Auctioning" },
                    new AuctionStatus { Name = "Lot Sold" },
                    new AuctionStatus { Name = "Over" },
                    new AuctionStatus { Name = "Not Started" }
                });
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new Category[]
                {
                    new Category { Name = "Clothes" },
                    new Category { Name = "Jewelry" },
                    new Category { Name = "Art" },
                    new Category { Name = "Games" },
                    new Category { Name = "Sculpture" },
                    new Category { Name = "Misc"},
                    new Category { Name = "Books"}
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
