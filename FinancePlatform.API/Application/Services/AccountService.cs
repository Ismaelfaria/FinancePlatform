using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using FluentValidation;
using Mapster;
using MapsterMapper;

namespace FinancePlatform.API.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<Account> _validator;
        private readonly IValidator<Guid> _guidValidator;
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;
        private const string CACHE_COLLECTION_KEY = "_AllAccounts";

        public AccountService(IAccountRepository accountRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IValidator<Account> validator,
                              IValidator<Guid> guidValidator,
                              IMapper mapper,
                              ICacheRepository cacheRepository)
        {
            _accountRepository = accountRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _validator = validator;
            _guidValidator = guidValidator;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }
        public async Task<List<AccountViewModel>?> FindAllAccountsAsync()
        {
            var accounts = await _cacheRepository.GetCollection<AccountViewModel>(CACHE_COLLECTION_KEY);

            if (accounts == null || !accounts.Any())
            {
                var existingAccounts = await _accountRepository.FindAllAsync();
                if (existingAccounts == null || existingAccounts.Count == 0)
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



        public async Task<Account?> CreateAccountAsync(AccountInputModel model)
        {
            var account = model.Adapt<Account>();
            var validationResult = _validator.Validate(account);

            if (!validationResult.IsValid)
                return null;

            var createdAccount = await _accountRepository.AddAsync(account);

            return createdAccount;
        }

        public async Task<Account?> UpdateAccountAsync(Guid accountId, Dictionary<string, object> updatedFields)
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
        public async Task<bool> DeleteAccountAsync(Guid accountId)
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
