using Auction.Business.Interfaces.Services;
using Auction.BusinessModels.Models;
using Auction.ApiModels.Lots.Requests;
using Auction.API.CurrentUserService;
using Auction.API.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using AutoMapper;
using Auction.Data.QueryFilters;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotsController : BaseController
    {
        public LotsController(ILotService lotService, CurrentUserAccessor currentUser, IMapper mapper) 
            : base(currentUser, mapper) => this.lotService = lotService;

        private readonly ILotService lotService;

        [HttpPost]
        [TranslateResultToActionResult]
        [Authorize(Roles = "Customer,Administrator")]
        public async Task<Result<int>> Create([FromForm]CreateLotRequest request, CancellationToken ct) =>
            await lotService.CreateNewLotAsync(currentUser.UserId, mapper.Map<NewLotModel>(request), ct);

        [HttpDelete("{id}")]
        [TranslateResultToActionResult]
        [Authorize(Roles = "Customer,Administrator")]
        public async Task<Result> Delete(int id, CancellationToken ct) => await lotService.DeleteLotAsync(id, ct);

        [HttpGet("{id}")]
        [TranslateResultToActionResult]
        public async Task<LotModel> Get(int id, CancellationToken ct) => await lotService.GetLotInfoByIdAsync(id, ct);

        [HttpGet("sale")]
        [TranslateResultToActionResult]
        public async Task<Result<IEnumerable<SaleLotModel>>> GetForSale([FromQuery]LotQueryFilter filter, CancellationToken ct) 
            => filter == null ? await lotService.GetForSaleAsync(ct) : await lotService.GetForSaleByFilterAsync(filter, ct);

        [HttpPut]
        [TranslateResultToActionResult]
        [Authorize(Roles = "Customer,Administrator")]
        public async Task<Result> Update(LotModel model, CancellationToken ct) => await lotService.UpdateLotAsync(model, ct);
    }
}
