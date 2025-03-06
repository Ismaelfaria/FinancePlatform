using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Application.Services.Cache;
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
        private readonly ICacheService _cacheService;

        public AccountService(IAccountRepository accountRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IValidator<Account> validator,
                              IValidator<Guid> guidValidator,
                              IMapper mapper,
                              ICacheService cacheService)
        {
            _accountRepository = accountRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _validator = validator;
            _guidValidator = guidValidator;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<List<AccountViewModel>?> FindAllAccountsAsync()
        {
            string accountListCacheKey = "accounts:list";

            var cachedAccounts = await _cacheService.GetAsync<List<Account>>(accountListCacheKey);
            if (cachedAccounts != null)
            {
                return _mapper.Map<List<AccountViewModel>>(cachedAccounts);
            }

            var existingAccounts = await _accountRepository.FindAllAsync();
            if (existingAccounts == null || existingAccounts.Count == 0)
            {
                return null;
            }

            await _cacheService.SetAsync(accountListCacheKey, existingAccounts);

            return _mapper.Map<List<AccountViewModel>>(existingAccounts);
        }

        public async Task<AccountViewModel?> FindByIdAsync(Guid accountId)
        {
            var validationResult = _guidValidator.Validate(accountId);

            if (!validationResult.IsValid) return null;

            string accountCacheKey = $"account:{accountId}";

            var cachedAccount = await _cacheService.GetAsync<Account>(accountCacheKey);
            
            if (cachedAccount != null)
            {
                return _mapper.Map<AccountViewModel>(cachedAccount);
            }

            var existingAccount = await _accountRepository.FindByIdAsync(accountId);
            
            if (existingAccount == null) return null;

            await _cacheService.SetAsync(accountCacheKey, existingAccount);

            return _mapper.Map<AccountViewModel>(existingAccount);
        }


        public async Task<Account?> CreateAccountAsync(AccountInputModel model)
        {
            var account = model.Adapt<Account>();
            var validationResult = _validator.Validate(account);

            if (!validationResult.IsValid) return null;

            var createdAccount = await _accountRepository.AddAsync(account);

            if (createdAccount != null)
            {
                string accountCacheKey = $"account:{createdAccount.Id}";

                await _cacheService.SetAsync(accountCacheKey, createdAccount);
            }
            return createdAccount;
        }
        public async Task<Account?> UpdateAccountAsync(Guid accountId, Dictionary<string, object> updatedFields)
        {
            var validationResult = _guidValidator.Validate(accountId);

            if (!validationResult.IsValid) return null;

            var account = await _accountRepository.FindByIdAsync(accountId);

            if (account == null) return null;

            var isUpdateSuccessful = _entityUpdateStrategy.UpdateEntityFields(account, updatedFields);
            
            if(isUpdateSuccessful)
            {
                await _accountRepository.UpdateAsync(account);

                string accountCacheKey = $"account:{accountId}";
                await _cacheService.UpdateCacheIfNeededAsync(accountCacheKey, account);
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

            string accountCacheKey = $"account:{accountId}";
            await _cacheService.RemoveFromCacheIfNeededAsync<Account>(accountCacheKey);

            return true;
        }
    }
}
