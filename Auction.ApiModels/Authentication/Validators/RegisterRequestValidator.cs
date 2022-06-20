using Auction.ApiModels.Authentication.Requests;
using FluentValidation;
using System;

namespace Auction.ApiModels.Authentication.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.BirthDate).Must(x => (DateTime.Today.Year - x.Year) >= 18);
        }
    }
}
