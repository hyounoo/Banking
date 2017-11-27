using Banking.Areas.Admin.ViewModels;
using FluentValidation;

namespace Banking.Areas.Admin.Validators
{
    public class ClientViewModelValidator : AbstractValidator<ClientViewModel>
    {
        public ClientViewModelValidator()
        {
            RuleFor(x => x.ClientName).NotNull();            
        }
    }
}