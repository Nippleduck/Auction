using Auction.Authentication.JWT;

namespace Auction.ApiModels.Authentication.Responses
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public AccessToken Token { get; set; }
    }
}
