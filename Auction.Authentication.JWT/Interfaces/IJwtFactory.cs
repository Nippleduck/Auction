using System.Threading.Tasks;

namespace Auction.Authentication.JWT.Interfaces
{
    public interface IJwtFactory
    {
        Task<(string, AccessToken)> GenerateEncodedTokenAsync(string userId, string email, string role);
        string GenerateRefreshToken(int size = 32);
    }
}
