using Banking.Areas.Admin.ViewModels;
using FluentValidation;

namespace Banking.Areas.Admin.Validators
{
    public class AccountSearchViewModelValidator : AbstractValidator<AccountSearchViewModel>
    {
        public AccountSearchViewModelValidator()
        {
            When(x => x.MinBalance != null && x.MaxBalance != null, () =>
            {
                RuleFor(x => x.MaxBalance).GreaterThanOrEqualTo(x => x.MinBalance).WithMessage("Maximum balance must be greater than or equal to Minimum balance.");
                RuleFor(x => x.MinBalance).LessThanOrEqualTo(x => x.MaxBalance).WithMessage("Minimum balance must be less than or equal to Maximum balance.");
            });
        }
    }
}