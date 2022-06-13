using Auction.Authentication.JWT.ConfigurationModels;
using Auction.Authentication.JWT.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Auction.Authentication.JWT.RegistrationExtensions
{
    public static class CustomTokenAuthenticationRegistration
    {
        public static IServiceCollection AddCustomTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddScoped<ITokenValidator, TokenValidator>();

            var authSettings = configuration.GetSection(nameof(ClientSecrets));
            services.Configure<ClientSecrets>(options => options.SecretKey = authSettings[nameof(ClientSecrets.SecretKey)]);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings[nameof(ClientSecrets.SecretKey)]));

            var jwtAppSettingsOptions = configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(config =>
            {
                config.ClaimsIssuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)];
                config.TokenValidationParameters = tokenValidationParameters;
                config.SaveToken = true;

            });

            services.AddAuthorization();

            return services;
        }
    }
}
