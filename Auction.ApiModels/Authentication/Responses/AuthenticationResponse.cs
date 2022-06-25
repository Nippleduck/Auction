﻿using Auction.Authentication.JWT;

namespace Auction.ApiModels.Authentication.Responses
{
    public class AuthenticationResponse
    {
        public string Role { get; set; }
        public string Username { get; set; }
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
