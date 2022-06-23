using Auction.ApiModels.Reviews.Requests;
using FluentValidation;
using System;

namespace Auction.ApiModels.Reviews.Validators
{
    public class ApprovePlacementValidator : AbstractValidator<ApprovePlacementRequest>
    {
        public ApprovePlacementValidator()
        {
            RuleFor(x => x.LotId).NotEmpty();
            RuleFor(x => x.OpenDate)
                .NotEmpty()
                .LessThan(x => x.CloseDate)
                .GreaterThan(DateTime.UtcNow - TimeSpan.FromMinutes(1));
            RuleFor(x => x.CloseDate)
                .NotEmpty()
                .GreaterThan(x => x.OpenDate);
        }
    }
}
