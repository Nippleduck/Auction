using Auction.ApiModels.Reviews.Requests;
using FluentValidation;

namespace Auction.ApiModels.Reviews.Validators
{
    public class ReapplyRequestValidator : AbstractValidator<ReapplyRequest>
    {
        public ReapplyRequestValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(300);
            RuleFor(x => x.StartPrice).LessThanOrEqualTo(500000);
        }
    }
}
