using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Auction.API.Controllers.Base;
using Auction.API.CurrentUserService;
using Auction.ApiModels.Bidding.Requests;
using Auction.Business.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiddingController : BaseController
    {
        public BiddingController(CurrentUserAccessor currentUser, IMapper mapper, IBiddingService biddingService)
            : base(currentUser, mapper) => this.biddingService = biddingService;

        private readonly IBiddingService biddingService;

        [HttpPost("place")]
        [TranslateResultToActionResult]
        [Authorize(Roles = "Customer")]
        public async Task<Result<int>> PlaceBid([FromBody] PlaceBidRequest request, CancellationToken ct) =>
            await biddingService.PlaceBidAsync(currentUser.UserId, request.LotId, request.Price, ct);

        [HttpPut("{lotId}/sold/{buyerId}")]
        [TranslateResultToActionResult]
        [Authorize(Roles = "Administrator")]
        public async Task<Result> ConfirmPurchase(int lotId, int buyerId, CancellationToken ct) => 
            await biddingService.ConfirmPurchaseAsync(lotId, buyerId, ct);
    }
}
