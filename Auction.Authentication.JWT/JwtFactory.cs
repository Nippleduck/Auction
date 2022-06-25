using Auction.Authentication.JWT.ConfigurationModels;
using Auction.Authentication.JWT.Interfaces;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace Auction.Authentication.JWT
{
    internal class JwtFactory : IJwtFactory
    {
        public JwtFactory(IOptions<JwtIssuerOptions> jwtIssuerOptions) =>
            this.jwtIssuerOptions = jwtIssuerOptions.Value;

        private readonly JwtIssuerOptions jwtIssuerOptions;

        public async Task<(string, AccessToken)> GenerateEncodedTokenAsync(string userId, string email, string role)
        {
            var tokenId = await jwtIssuerOptions.GenerateJti();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                new Claim(JwtRegisteredClaimNames.Iat, jwtIssuerOptions.IssuedAt.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role)
            };

            var jwt = new JwtSecurityToken(
                jwtIssuerOptions.Issuer,
                jwtIssuerOptions.Audience,
                claims,
                jwtIssuerOptions.NotBefore,
                jwtIssuerOptions.Expiration,
                jwtIssuerOptions.SigningCredentials
                );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var validFor = (int)jwtIssuerOptions.ValidFor.TotalSeconds;

            return (tokenId, new AccessToken(encodedToken, validFor));
        }

        public string GenerateRefreshToken(int size = 32)
        {
            var randomNumber = new byte[size];

            using var random = RandomNumberGenerator.Create();
            random.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
