using FluentValidation;

namespace FinancePlatform.API.Application.Validators
{
    public class ValidatorGuid : AbstractValidator<Guid?>
    {
        public ValidatorGuid()
        {
            RuleFor(id => id)
                .NotNull().WithMessage("ID não pode ser nulo.")
                .NotEqual(Guid.Empty).WithMessage("ID não pode ser um GUID vazio.");
        }
    }
}
