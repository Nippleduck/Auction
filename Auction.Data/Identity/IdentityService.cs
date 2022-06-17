using Auction.TransferObjects.Authentication.Requests;
using Auction.TransferObjects.Authentication.Responses;
using Auction.Authentication.JWT.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Ardalis.Result;
using System.Linq;

namespace Auction.Data.Identity
{
    public class IdentityService
    {
        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtFactory jwtFactory)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtFactory = jwtFactory;
        }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IJwtFactory jwtFactory;

        public async Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null) return Result.NotFound();

            var signIn = await signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if (!signIn.Succeeded) return Result.Error($"Invalid credentials for '{request.Email}'");

            var roles = await userManager.GetRolesAsync(user);
            var token = await jwtFactory.GenerateEncodedTokenAsync(user.Id, user.Email, roles.First());

            var response = new AuthenticationResponse
            {
                Id = user.Id,
                Email = user.Email,
                Token = token
            };

            return Result.Success(response, $"user {user.Email} successfully authenticated");
        }

        public async Task<Result<bool>> RegisterAsync(RegisterRequest request)
        {
            var withSameEmail = await userManager.FindByEmailAsync(request.Email);

            if (withSameEmail != null) return Result.Error("User with specified email exists");

            var user = new ApplicationUser { Email = request.Email };
            var create = await userManager.CreateAsync(user, request.Password);

            if (!create.Succeeded) return Result.Error(create.Errors.Select(e => e.Description).ToArray());

            await userManager.AddToRoleAsync(user, Roles.Customer.ToString());

            return Result.Success(true, "user successfully registered");
        }
    }
}
