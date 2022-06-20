using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;

namespace Auction.ApiModels
{
    public static class ValidatorsRegistration
    {
        public static IServiceCollection AddApiModelsValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
}
