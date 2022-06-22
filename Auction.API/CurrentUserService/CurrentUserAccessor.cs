using Auction.Data.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Auction.API.CurrentUserService
{
    public class CurrentUserAccessor
    {
        public CurrentUserAccessor(IHttpContextAccessor contextAccessor)
        {
            UserId = int.Parse(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            IsAdmin = contextAccessor.HttpContext.User.IsInRole(Roles.Administrator.ToString());
        }

        public int UserId { get; }
        public bool IsAdmin { get; }
    }
}
