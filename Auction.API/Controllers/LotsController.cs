using Auction.ApiModels.Lots.Requests;
using Auction.Business.Interfaces.Services;
using Auction.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotsController : ControllerBase
    {
        public LotsController(ILotService lotService) => this.lotService = lotService;

        private readonly ILotService lotService;

        [HttpPost("{id}/create")]
        [Authorize(Roles = "Customer,Administrator")]
        public async Task<ActionResult<int>> CreateAsync(int id, [FromForm]CreateLotRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newLot = new NewLotModel
            {
                SellerId = id,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                StatusId = request.StatusId,
                StartPrice = request.StartPrice,
                Image = new ImageModel
                {
                    FileName = request.Image.FileName,
                    Type = request.Image.ContentType,
                    Content = request.Image.OpenReadStream()
                }
            };

            var lotId = await lotService.CreateNewLotAsync(newLot, ct);

            return Ok(lotId.Value);
        }

        [HttpDelete("{id}")]
        [TranslateResultToActionResult]
        [Authorize(Roles = "Customer,Administrator")]
        public async Task<Result> DeleteAsync(int id, CancellationToken ct) => await lotService.DeleteLotAsync(id, ct);

        [HttpGet("{id}")]
        [TranslateResultToActionResult]
        public async Task<LotModel> GetAsync(int id, CancellationToken ct) => await lotService.GetLotInfoByIdAsync(id, ct);

        [HttpGet("sale")]
        [TranslateResultToActionResult]
        public async Task<Result<IEnumerable<LotModel>>> GetForSaleAsync(CancellationToken ct) => await lotService.GetForSaleAsync(ct);
    }
}
