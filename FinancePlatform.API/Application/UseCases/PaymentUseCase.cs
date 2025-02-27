using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.UseCases;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FluentValidation;
using Mapster;
using MapsterMapper;

namespace FinancePlatform.API.Application.UseCases
{
    public class PaymentUseCase : IPaymentUseCase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<Payment> _validator;
        private readonly IMapper _mapper;

        public PaymentUseCase(IPaymentRepository paymentRepository, 
                              IAccountRepository accountRepository,
                              IValidator<Payment> validator,
                              IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Payment> generatePayment(PaymentInputModel model)
        {
            var payment = model.Adapt<Payment>();
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
