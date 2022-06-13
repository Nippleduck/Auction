using System.Security.Claims;

namespace Auction.Authentication.JWT.Interfaces
{
    public interface ITokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
