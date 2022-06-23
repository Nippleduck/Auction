using Auction.ApiModels.Bidding.Requests;
using FluentValidation;

namespace Auction.ApiModels.Bidding.Validators
{
    public class PlaceBidValidator : AbstractValidator<PlaceBidRequest>
    {
        public PlaceBidValidator()
        {
            RuleFor(x => x.LotId).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
        }
    }
}
