using Auction.Authentication.JWT;

namespace Auction.ApiModels.Authentication.Responses
{
    public class RefreshTokenResponse
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
