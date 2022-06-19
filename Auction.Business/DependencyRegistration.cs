using Auction.Business.ImageProcessing;
using Auction.Business.Interfaces;
using Auction.Business.Interfaces.Services;
using Auction.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auction.Business
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddBusinessDependencies(this IServiceCollection services)
        {
            services.AddTransient<IImageConverter, ImageConverter>();

            services.AddTransient<ILotService, LotService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
