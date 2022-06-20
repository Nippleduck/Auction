using Auction.Authentication.JWT.RegistrationExtensions;
using Auction.Business;
using Auction.Data;
using Auction.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Auction.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCustomTokenAuthentication(Configuration);
            services.AddDataDependencies(Configuration);
            services.AddBusinessDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using var scope = app.ApplicationServices.CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<DbContextSeeder>();

                initializer.InitializeAsync().GetAwaiter().GetResult();
                initializer.TrySeedAsync().GetAwaiter().GetResult();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
