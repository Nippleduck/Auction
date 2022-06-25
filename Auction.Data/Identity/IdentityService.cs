using Auction.Authentication.JWT.ConfigurationModels;
using Auction.Authentication.JWT.Interfaces;
using Auction.ApiModels.Authentication.Requests;
using Auction.ApiModels.Authentication.Responses;
using Auction.Data.Identity.Models;
using Auction.Data.Context;
using Auction.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Linq;
using Ardalis.Result;
using Auction.Authentication.JWT;
using System;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Auction.Data.Identity
{
    public class IdentityService
    {
        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AuctionContext context,
            ITokenValidator tokenValidator,
            IOptions<ClientSecrets> options,
            IJwtFactory jwtFactory)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.tokenValidator = tokenValidator;
            this.options = options.Value;
            this.jwtFactory = jwtFactory;
        }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AuctionContext context;
        private readonly ClientSecrets options;
        private readonly ITokenValidator tokenValidator;
        private readonly IJwtFactory jwtFactory;

        public async Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null) return Result.NotFound();

            var signIn = await signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if (!signIn.Succeeded) return Result.Error($"Invalid credentials for '{request.Email}'");

            var roles = await userManager.GetRolesAsync(user);
            var (token, refreshToken) = await GenerateTokenBundle(user);

            var response = new AuthenticationResponse
            {
                Role = roles.First(),
                Username = user.UserName,
                AccessToken = token,
                RefreshToken = refreshToken
            };

            return Result.Success(response, $"User {user.Email} successfully authenticated");
        }

        public async Task<Result<bool>> RegisterAsync(RegisterRequest request)
        {
            var withSameEmail = await userManager.FindByEmailAsync(request.Email);

            if (withSameEmail != null) return Result.Error("User with specified email exists");

            var person = new Person 
            { 
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = request.BirthDate 
            };

            var user = new ApplicationUser 
            { 
                Email = request.Email,
                UserName = $"{person.Name}{person.Surname}",
                Person = person
            };

            var create = await userManager.CreateAsync(user, request.Password);

            if (!create.Succeeded) return Result.Error(create.Errors.Select(e => e.Description).ToArray());

            await userManager.AddToRoleAsync(user, Roles.Customer.ToString());

            return Result.Success(true, "User successfully registered");
        }

        public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var validated = tokenValidator.GetPrincipalFromToken(request.AccessToken, options.SecretKey);

            if (validated == null) return Result.Error("Token is invalid");

            var jti = validated.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedToken = await context.RefreshTokens
                .SingleOrDefaultAsync(t => t.Token.ToString() == request.RefreshToken);

            if (storedToken == null 
                || storedToken.ExpirationDate < DateTime.UtcNow
                || storedToken.JwtId != jti)
            {
                return Result.Error("No valid unexpired token");
            }

            context.RefreshTokens.Remove(storedToken);
            await context.SaveChangesAsync();

            var email = validated.Claims.First(c => c.Properties.First().Value == JwtRegisteredClaimNames.Email).Value;
            var user = await userManager.FindByEmailAsync(email);

            if (user == null) return Result.Error("principal does not represent correct user");

            var (token, refreshToken) = await GenerateTokenBundle(user);

            return Result.Success(new RefreshTokenResponse { AccessToken = token, RefreshToken = refreshToken });
        }

        private async Task<(AccessToken, string)> GenerateTokenBundle(ApplicationUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            var (tokenId, token) = await jwtFactory.GenerateEncodedTokenAsync(user.PersonId.ToString(), user.Email, roles.First());

            var refreshToken = new RefreshToken
            {
                JwtId = tokenId,
                UserId = user.Id,
                ExpirationDate = DateTime.UtcNow.AddDays(1)
            };

            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();

            return (token, refreshToken.Token.ToString());
        }
    }
}
