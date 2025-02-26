using FinancePlatform.API.Domain.Entities;
using FluentValidation;

namespace FinancePlatform.API.Application.Validators
{
    public class ValidatorReconciliation : AbstractValidator<Reconciliation>
    {
        public ValidatorReconciliation()
        {
            RuleFor(r => r.PaymentId)
                .NotEmpty().WithMessage("O PaymentId é obrigatório");

            RuleFor(r => r.ProcessedAt)
                .NotEmpty().WithMessage("A data de processamento é obrigatória")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data de processamento não pode ser no futuro");

            RuleFor(r => r.IsSuccessful)
                .NotNull().WithMessage("O campo IsSuccessful é obrigatório");
        }
    }
}
