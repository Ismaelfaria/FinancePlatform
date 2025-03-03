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
        private readonly IEntityUpdateStrategy _entityUpdateStrategy;
        private readonly IMapper _mapper;
        private readonly CacheService _cacheService;

        public AccountService(IAccountRepository accountRepository,
                              IEntityUpdateStrategy entityUpdateStrategy,
                              IValidator<Account> validator,
                              IMapper mapper,
                              CacheService cacheService)
        {
            _accountRepository = accountRepository;
            _entityUpdateStrategy = entityUpdateStrategy;
            _validator = validator;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<List<AccountViewModel>> FindAllAccountsAsync()
        {
            string cacheKey = "accounts:list";

            var cachedAccounts = await _cacheService.GetAsync<List<Account>>(cacheKey);
            if (cachedAccounts != null)
            {
                return _mapper.Map<List<AccountViewModel>>(cachedAccounts);
            }

            var accountsFromDb = await _accountRepository.FindAllAsync();
            if (accountsFromDb == null || !accountsFromDb.Any())
            {
                return null;
            }

            await _cacheService.SetAsync(cacheKey, accountsFromDb);

            return _mapper.Map<List<AccountViewModel>>(accountsFromDb);
        }

        public async Task<AccountViewModel> FindByIdAsync(Guid accountId)
        {
            string cacheKey = $"account:{accountId}";

            var cachedAccount = await _cacheService.GetAsync<Account>(cacheKey);
            if (cachedAccount != null)
            {
                return _mapper.Map<AccountViewModel>(cachedAccount);
            }

            var accountFromDb = await _accountRepository.FindByIdAsync(accountId);
            if (accountFromDb == null)
            {
                return null;
            }

            await _cacheService.SetAsync(cacheKey, accountFromDb);

            return _mapper.Map<AccountViewModel>(accountFromDb);
        }


        public async Task<Account> CreateAccountAsync(AccountInputModel model)
        {
            var account = model.Adapt<Account>();
            var validator = _validator.Validate(account);

            if (!validator.IsValid) return null;

            var createdAccount = await _accountRepository.AddAsync(account);

            if (createdAccount != null)
            {
                string cacheKey = $"account:{createdAccount.Id}";

                await _cacheService.SetAsync(cacheKey, createdAccount);
            }

            return createdAccount;
        }
        public async Task<Account> UpdateAsync(Guid accountId, Dictionary<string, object> updateRequest)
        {
            Account account = await _accountRepository.FindByIdAsync(accountId);

            if (account == null) return null;

            _entityUpdateStrategy.UpdateEntityFields(account, updateRequest);

            await _accountRepository.UpdateAsync(account);

            return account;
        }
        public async Task<bool> DeleteAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.FindByIdAsync(accountId);
            if (account == null) return false;

            _accountRepository.Delete(account);
            return true;
        }
    }
}
