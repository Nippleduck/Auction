using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Auction.ApiModels.Authentication.Requests;
using Auction.ApiModels.Authentication.Responses;
using Auction.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(IdentityService identityService) => this.identityService = identityService;

        private readonly IdentityService identityService;

        [HttpPost]
        [TranslateResultToActionResult]
        public async Task<Result<bool>> Register([FromBody]RegisterRequest request) =>
            await identityService.RegisterAsync(request);


        [HttpGet]
        [TranslateResultToActionResult]
        public async Task<Result<AuthenticationResponse>> Authenticate([FromBody] AuthenticationRequest request) =>
            await identityService.AuthenticateAsync(request);
    }
}
