using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;

namespace Auction.ApiModels
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddApiModels(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
