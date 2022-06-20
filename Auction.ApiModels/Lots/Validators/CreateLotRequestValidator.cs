using Auction.ApiModels.Lots.Requests;
using FluentValidation;

namespace Auction.ApiModels.Lots.Validators
{
    public class CreateLotRequestValidator : AbstractValidator<CreateLotRequest>
    {
        public CreateLotRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.StartPrice).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty(); 
            RuleFor(x => x.StatusId).NotEmpty();
        }
    }
}
