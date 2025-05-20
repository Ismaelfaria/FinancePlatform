using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using Moq;
using Xunit;

namespace FinancePlatform.API.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<Account> _validator;
        private readonly IValidator<AccountInputModel> _validatorAccountInputModel;
        private readonly IValidator<Guid> _guidValidator;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;
        private const string CACHE_COLLECTION_KEY = "_AllAccounts";

        public AccountService(IAccountRepository accountRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IValidator<Account> validator,
                              IValidator<AccountInputModel> validatorAccountInputModel,
                              IValidator<Guid> guidValidator,
                              IMapper mapper,
                              ICacheRepository cacheRepository)
        {
            _accountRepository = accountRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _validator = validator;
            _validatorAccountInputModel = validatorAccountInputModel;
            _guidValidator = guidValidator;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }
        public async Task<List<AccountViewModel>?> FindAllAsync()
        {
            var accounts = await _cacheRepository.GetCollection<AccountViewModel>(CACHE_COLLECTION_KEY);

            if (accounts == null || !accounts.Any())
            {
                var existingAccounts = await _accountRepository.FindAllAsync();
                if (existingAccounts == null || !existingAccounts.Any())
                {
                    return null;
                }

                var accountViewModels = _mapper.Map<List<AccountViewModel>>(existingAccounts);
                await _cacheRepository.SetCollection(CACHE_COLLECTION_KEY, accountViewModels);
                return accountViewModels;
            }

            return _mapper.Map<List<AccountViewModel>>(accounts);
        }

        public async Task<AccountViewModel?> FindByIdAsync(Guid accountId)
        {
            var validationResult = _guidValidator.Validate(accountId);
            if (!validationResult.IsValid) return null;

            var account = await _cacheRepository.GetValue<AccountViewModel>(accountId);

            if (account == null)
            {
                var existingAccount = await _accountRepository.FindByIdAsync(accountId);
                if (existingAccount == null) return null;

                var accountViewModel = _mapper.Map<AccountViewModel>(existingAccount);
                await _cacheRepository.SetValue(accountId, accountViewModel);
                return accountViewModel;
            }

            return _mapper.Map<AccountViewModel>(account);
        }

        public async Task<Account?> AddAsync(AccountInputModel model)
        {
            var validationResult = _validatorAccountInputModel.Validate(model);

            if (validationResult != null)
                return null;

            var account = model.Adapt<Account>();

            var createdAccount = await _accountRepository.AddAsync(account);

            return createdAccount;
        }

        public async Task<Account?> UpdateAsync(Guid accountId, Dictionary<string, object> updatedFields)
        {
            var validationResult = _guidValidator.Validate(accountId);

            if (!validationResult.IsValid) return null;

            var account = await _accountRepository.FindByIdAsync(accountId);
            if (account == null) return null;

            var isUpdateSuccessful = _entityUpdateStrategy.UpdateEntityFields(account, updatedFields);
            if (isUpdateSuccessful)
            {
                await _accountRepository.UpdateAsync(account);
            }
            return account;
        }
        public async Task<bool> DeleteAsync(Guid accountId)
        {
            var validationResult = _guidValidator.Validate(accountId);

            if (!validationResult.IsValid) return false;

            var account = await _accountRepository.FindByIdAsync(accountId);

            if (account == null) return false;

            _accountRepository.Delete(account);

            return true;
        }
    }
}
