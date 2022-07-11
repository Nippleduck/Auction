using Auction.ApiModels.Lots.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auction.ApiModels.Lots.Validators
{
    public class CreateAdminLotRequestValidator : AbstractValidator<CreateAdminLotRequest>
    {
        public CreateAdminLotRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
            RuleFor(x => x.StartPrice).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.StatusId).NotEmpty();
            RuleFor(x => x.OpenDate).GreaterThanOrEqualTo(DateTime.Now - TimeSpan.FromMinutes(1));
            RuleFor(x => x.CloseDate).GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(x => x.MinimalBid).LessThanOrEqualTo(10000);
        }
    }
}
