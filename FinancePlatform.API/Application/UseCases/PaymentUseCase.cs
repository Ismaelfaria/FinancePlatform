using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.UseCases;
using FinancePlatform.API.Domain.Entities;
using FluentValidation;

namespace FinancePlatform.API.Application.UseCases
{
    public class PaymentUseCase : IPaymentUseCase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<Payment> _validator;

        public PaymentUseCase(IPaymentRepository paymentRepository, 
                              IAccountRepository accountRepository,
                              IValidator<Payment> validator)
        {
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
            _validator = validator;
        }

        public async Task<Payment> generatePayment(Payment payment)
        {
            var validator = _validator.Validate(payment);

            if (!validator.IsValid) return null;

            var account = await _accountRepository.FindByIdAsync(payment.AccountId);
            if (account == null) return null;

            if (payment.Amount > account.Balance)
            {
                payment.Reject();
            }
            else
            {
                payment.Approve();
                account.Debit(payment.Amount);
                await _accountRepository.UpdateAsync(account);
            }
            return await _paymentRepository.AddAsync(payment);
        }
    }
}
