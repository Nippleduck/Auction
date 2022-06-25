using Auction.Data.Identity.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Auction.API.CurrentUserService
{
    public class CurrentUserAccessor
    {
        public CurrentUserAccessor(IHttpContextAccessor contextAccessor)
        {
            UserId = int.Parse(contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var hasAdminClaim = contextAccessor.HttpContext?.User?.IsInRole(Roles.Administrator.ToString());
            IsAdmin = hasAdminClaim ?? false;
        }

        public int UserId { get; }
        public bool IsAdmin { get; }
    }
}
