using Auction.Data.Context;
using Auction.Data.Identity;
using Auction.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Data
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuctionContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AuctionDB"),
                builder => builder.MigrationsAssembly(typeof(AuctionContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuctionContext>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IdentityService>();

            services.AddScoped<DbContextSeeder>();

            return services;
        }
    }
}
