using Auction.ApiModels.Lots.Requests;
using FluentValidation;
using System;

namespace Auction.ApiModels.Lots.Validators
{
    public class UpdateBiddingDetailsRequestValidator : AbstractValidator<UpdateBiddingDetailsRequest>
    {
        public UpdateBiddingDetailsRequestValidator()
        {
            RuleFor(x => x.OpenDate).GreaterThanOrEqualTo(DateTime.Now - TimeSpan.FromMinutes(1));
            RuleFor(x => x.CloseDate).GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(x => x.MinimalBid).LessThanOrEqualTo(10000);
        }
    }
}
