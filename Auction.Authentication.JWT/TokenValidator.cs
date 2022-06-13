using Auction.Authentication.JWT.Exceptions;
using Auction.Authentication.JWT.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auction.Authentication.JWT
{
    public class TokenValidator : ITokenValidator
    {
        public TokenValidator() => jwtSecurityTokenHandler ??= new JwtSecurityTokenHandler();

        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateLifetime = false
            };

            var principal = jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || !SignatureAlgorithmValid(jwtSecurityToken))
                throw new TokenValidationException("Exception occured while validating token. Check token parameters");

            return principal;    
        }

        private bool SignatureAlgorithmValid(JwtSecurityToken token) =>
            token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }
}
