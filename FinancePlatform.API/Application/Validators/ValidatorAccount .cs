using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FluentValidation;

namespace FinancePlatform.API.Application.Validators
{
    public class ValidatorAccount : AbstractValidator<Account>
    {
        public ValidatorAccount()
        {
            RuleFor(a => a.HolderName)
                .NotEmpty().WithMessage("O nome do titular é obrigatório")
                .Length(3, 100).WithMessage("O nome do titular deve ter entre 3 e 100 caracteres")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Username deve conter apenas caracteres alfanuméricos.");

            RuleFor(a => a.AccountNumber)
                .NotEmpty().WithMessage("O número da conta é obrigatório")
                .Matches(@"^\d{10,15}$").WithMessage("O número da conta deve ter entre 10 e 15 dígitos numéricos");

            RuleFor(a => a.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("O saldo da conta não pode ser negativo");
        }
    }


    public class ValidatorAccountIM : AbstractValidator<AccountInputModel>
    {
        public ValidatorAccountIM()
        {
            RuleFor(a => a.HolderName)
                .NotEmpty().WithMessage("O nome do titular é obrigatório")
                .Length(3, 100).WithMessage("O nome do titular deve ter entre 3 e 100 caracteres")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Username deve conter apenas caracteres alfanuméricos.");

            RuleFor(a => a.AccountNumber)
                .NotEmpty().WithMessage("O número da conta é obrigatório")
                .Matches(@"^\d{10,15}$").WithMessage("O número da conta deve ter entre 10 e 15 dígitos numéricos");

            RuleFor(a => a.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("O saldo da conta não pode ser negativo");
        }
    }
}
