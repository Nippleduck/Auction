using Auction.ApiModels.Lots.Requests;
using FluentValidation;

namespace Auction.ApiModels.Lots.Validators
{
    public class UpdateBiddingDetailsRequestValidator : AbstractValidator<UpdateBiddingDetailsRequest>
    {
        public UpdateBiddingDetailsRequestValidator()
        {
            RuleFor(x => x.LotId).NotEmpty();
        }
    }
}
