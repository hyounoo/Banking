using Banking.Areas.Admin.ViewModels;
using FluentValidation;

namespace Banking.Areas.Admin.Validators
{
    public class AccountViewModelValidator : AbstractValidator<AccountViewModel>
    {
        public AccountViewModelValidator()
        {
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.ClientId).NotNull();
            RuleFor(x => x.CurrencyId).NotNull();
        }
    }
}