using Auction.ApiModels.Lots.Requests;
using FluentValidation;

namespace Auction.ApiModels.Lots.Validators
{
    public class UpdateLotDetailsRequestValidator : AbstractValidator<UpdateLotDetailsRequest>
    {
        public UpdateLotDetailsRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MaximumLength(40);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
        }
    }
}
