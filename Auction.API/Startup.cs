using Auction.Authentication.JWT.RegistrationExtensions;
using Auction.API.CurrentUserService;
using Auction.ApiModels;
using Auction.Business;
using Auction.Data.Context;
using Auction.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using Auction.BusinessModels;

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
            services.AddControllers().AddFluentValidation();

            services.AddDataDependencies(Configuration);
            services.AddBusinessDependencies();
            services.AddCustomTokenAuthentication(Configuration);
            services.AddApiModels();
            services.AddBusinessModels();

            services.AddScoped<CurrentUserAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //using var scope = app.ApplicationServices.CreateScope();
                //var initializer = scope.ServiceProvider.GetRequiredService<DbContextSeeder>();

                //initializer.InitializeAsync().GetAwaiter().GetResult();
                //initializer.TrySeedAsync().GetAwaiter().GetResult();
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
