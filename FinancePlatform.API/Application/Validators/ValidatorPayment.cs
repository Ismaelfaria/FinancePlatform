using FinancePlatform.API.Domain.Entities;
using FluentValidation;

namespace FinancePlatform.API.Application.Validators
{
    public class ValidatorPayment : AbstractValidator<Payment>
    {
        public ValidatorPayment()
        {
            RuleFor(p => p.AccountId)
            .NotEmpty().WithMessage("O AccountId é obrigatório");

            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero");

            RuleFor(p => p.Status)
                .IsInEnum().WithMessage("O Status do pagamento é inválido");
        }
    }
}
