using Auction.ApiModels.Lots.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotsController : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> CreateLotAsync([FromForm]CreateLotRequest request)
        {

        }
    }
}
