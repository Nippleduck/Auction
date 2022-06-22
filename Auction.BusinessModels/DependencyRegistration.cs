using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auction.BusinessModels
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddBusinessModels(this IServiceCollection services) =>
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
