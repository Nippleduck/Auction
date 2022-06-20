using Auction.ApiModels.Authentication.Requests;
using Auction.ApiModels.Authentication.Responses;
using Auction.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(IdentityService identityService)
        {
            this.identityService = identityService;
        }

        private readonly IdentityService identityService;

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody]RegisterRequest request)
        {
            //if (!ModelState.IsValid) return BadRequest();

            var result = await identityService.RegisterAsync(request);

            return Ok(result.Value);
        }

        [HttpGet("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            var response = await identityService.AuthenticateAsync(request);

            return Ok(response.Value);
        }
    }
}
