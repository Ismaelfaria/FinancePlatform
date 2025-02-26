using FinancePlatform.API.Domain.Entities;
using FluentValidation;

namespace FinancePlatform.API.Application.Validators
{
    public class ValidatorNotification : AbstractValidator<Notification>
    {
        public ValidatorNotification()
        {
            RuleFor(n => n.AccountId)
                .NotEmpty().WithMessage("O AccountId é obrigatório");

            RuleFor(n => n.Message)
                .NotEmpty().WithMessage("A mensagem é obrigatória")
                .Length(5, 500).WithMessage("A mensagem deve ter entre 5 e 500 caracteres");

            RuleFor(n => n.Type)
                .IsInEnum().WithMessage("O tipo de notificação é inválido");
        }
    }
}
