using Auction.ApiModels.Lots.Requests;
using Auction.API.Controllers.Base;
using Auction.Business.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Auction.API.CurrentUserService;
using Auction.BusinessModels.Models;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotsController : BaseController
    {
        public LotsController(ILotService lotService, CurrentUserAccessor currentUser) 
            : base(currentUser) => this.lotService = lotService;

        private readonly ILotService lotService;

        [HttpPost]
        [Authorize(Roles = "Customer,Administrator")]
        [TranslateResultToActionResult]
        public async Task<Result<int>> Create([FromForm]CreateLotRequest request, CancellationToken ct)
        {
            var newLot = new NewLotModel
            {
                SellerId = currentUser.UserId,
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

            return await lotService.CreateNewLotAsync(newLot, ct);
        }

        [HttpDelete("{id}")]
        [TranslateResultToActionResult]
        [Authorize(Roles = "Customer,Administrator")]
        public async Task<Result> Delete(int id, CancellationToken ct) => await lotService.DeleteLotAsync(id, ct);

        [HttpGet("{id}")]
        [TranslateResultToActionResult]
        public async Task<LotModel> Get(int id, CancellationToken ct) => await lotService.GetLotInfoByIdAsync(id, ct);

        [HttpGet("sale")]
        [TranslateResultToActionResult]
        public async Task<Result<IEnumerable<SaleLotModel>>> GetForSale(CancellationToken ct) => await lotService.GetForSaleAsync(ct);

        [HttpPut]
        [TranslateResultToActionResult]
        [Authorize(Roles = "Customer,Administrator")]
        public async Task<Result> Update(LotModel model, CancellationToken ct) => await lotService.UpdateLotAsync(model, ct);
    }
}
