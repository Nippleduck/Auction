using System.Threading.Tasks;

namespace Auction.Authentication.JWT.Interfaces
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedTokenAsync(string userId, string email, string role);
        string GenerateRefreshToken(int size = 32);
    }
}
