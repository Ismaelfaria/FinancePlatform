using Moq;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FluentValidation;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Services;
using FinancePlatform.API.Presentation.DTOs.ViewModel;
using MapsterMapper;
using FluentValidation.Results;
using FinancePlatform.API.Presentation.DTOs.InputModel;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IValidator<Account>> _validatorMock;
    private readonly Mock<IValidator<Guid>> _guidValidatorMock;
    private readonly Mock<IEntityUpdateStrategy> _entityUpdateStrategyMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheRepository> _cacheRepositoryMock;
    private readonly AccountService _accountService;

    private const string CACHE_COLLECTION_KEY = "_AllAccounts";

    public AccountServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _validatorMock = new Mock<IValidator<Account>>();
        _guidValidatorMock = new Mock<IValidator<Guid>>();
        _entityUpdateStrategyMock = new Mock<IEntityUpdateStrategy>();
        _mapperMock = new Mock<IMapper>();
        _cacheRepositoryMock = new Mock<ICacheRepository>();

        _accountService = new AccountService(
            _accountRepositoryMock.Object,
            _entityUpdateStrategyMock.Object,
            _validatorMock.Object,
            _guidValidatorMock.Object,
            _mapperMock.Object,
            _cacheRepositoryMock.Object
        );
    }

    [Fact]
    public async Task FindAllAsync_ShouldStoreDataInCache_WhenRepositoryReturnsData()
    {
        _cacheRepositoryMock.Setup(repo => repo.GetCollection<AccountViewModel>(CACHE_COLLECTION_KEY))
                            .ReturnsAsync((List<AccountViewModel>?)null);

        var existingAccounts = new List<Account>
        {
            new Account
            {
                Id = Guid.NewGuid(),
                HolderName = "Alice Smith",
                AccountNumber = "987123456",
                Balance = 5000.00m
            },
            new Account
            {
                Id = Guid.NewGuid(),
                HolderName = "Bob Johnson",
                AccountNumber = "123789456",
                Balance = 750.25m
            }
       };

        _accountRepositoryMock.Setup(repo => repo.FindAllAsync())
                              .ReturnsAsync(existingAccounts);

        _mapperMock.Setup(m => m.Map<List<AccountViewModel>>(existingAccounts)).Returns(existingAccounts
            .Select(acc => new AccountViewModel
            {
                Id = acc.Id,
                HolderName = acc.HolderName,
                AccountNumber = acc.AccountNumber,
                Balance = acc.Balance
            }).ToList());

        var result = await _accountService.FindAllAsync();

        Assert.NotNull(result);
        Assert.Equal(existingAccounts.Count, result.Count);
        _cacheRepositoryMock.Verify(repo => repo.SetCollection(CACHE_COLLECTION_KEY, It.IsAny<List<AccountViewModel>>()), Times.Once);
    }

    [Fact]
    public async Task FindAllAsync_ShouldReturnNull_WhenCacheAndRepositoryAreEmpty()
    {
        _cacheRepositoryMock.Setup(repo => repo.GetCollection<AccountViewModel>(CACHE_COLLECTION_KEY))
                            .ReturnsAsync((List<AccountViewModel>?)null);
        _accountRepositoryMock.Setup(repo => repo.FindAllAsync())
                              .ReturnsAsync(new List<Account>());
        _mapperMock.Setup(m => m.Map<List<AccountViewModel>>(It.IsAny<List<Account>>()))
                              .Returns(new List<AccountViewModel>());

        var result = await _accountService.FindAllAsync();

        Assert.Null(result);
    }


    [Fact]
    public async Task FindAllAsync_ShouldReturnNull_WhenRepositoryHasNoData()
    {
        _cacheRepositoryMock.Setup(repo => repo.GetCollection<AccountViewModel>(CACHE_COLLECTION_KEY))
                            .ReturnsAsync((List<AccountViewModel>?)null);
        _accountRepositoryMock.Setup(repo => repo.FindAllAsync())
                              .ReturnsAsync((List<Account>?)null);

        var result = await _accountService.FindAllAsync();

        Assert.Null(result);
    }

    [Fact]
    public async Task FindByIdAsync_InvalidGuid_ReturnsNull()
    {
        var invalidResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Id", "Invalid GUID")
        });

        _guidValidatorMock
            .Setup(v => v.Validate(It.IsAny<Guid>()))
            .Returns(invalidResult);

        var result = await _accountService.FindByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }
    [Fact]
    public async Task FindByIdAsync_GuidValid_ButNotFoundAnywhere_ReturnsNull()
    {
        var validResult = new ValidationResult();
        var accountId = Guid.NewGuid();

        _guidValidatorMock.Setup(v => v.Validate(accountId)).Returns(validResult);
        _cacheRepositoryMock.Setup(c => c.GetValue<AccountViewModel>(accountId)).ReturnsAsync((AccountViewModel?)null);
        _accountRepositoryMock.Setup(r => r.FindByIdAsync(accountId)).ReturnsAsync((Account?)null);

        var result = await _accountService.FindByIdAsync(accountId);

        Assert.Null(result);
    }

    [Fact]
    public async Task FindByIdAsync_NotInCache_FoundInDatabase_ReturnsMappedResult()
    {
        var validResult = new ValidationResult();
        var accountId = Guid.NewGuid();
        var entity = new Account { Id = accountId, HolderName = "DbUser" };
        var viewModel = new AccountViewModel { HolderName = "DbUser" };

        _guidValidatorMock.Setup(v => v.Validate(accountId)).Returns(validResult);
        _cacheRepositoryMock.Setup(c => c.GetValue<AccountViewModel>(accountId))
            .ReturnsAsync((AccountViewModel?)null);
        _accountRepositoryMock.Setup(r => r.FindByIdAsync(accountId)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<AccountViewModel>(entity)).Returns(viewModel);

        var result = await _accountService.FindByIdAsync(accountId);

        Assert.NotNull(result);
        Assert.Equal(viewModel.HolderName, result.HolderName);

        _cacheRepositoryMock.Verify(c => c.SetValue(accountId, viewModel), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_FoundInCache_ReturnsMappedResult()
    {
        var validResult = new ValidationResult();
        var accountId = Guid.NewGuid();
        var cached = new AccountViewModel { HolderName = "CachedUser" };

        _guidValidatorMock.Setup(v => v.Validate(accountId)).Returns(validResult);
        _cacheRepositoryMock.Setup(c => c.GetValue<AccountViewModel>(accountId)).ReturnsAsync(cached);
        _mapperMock.Setup(m => m.Map<AccountViewModel>(cached)).Returns(cached); 

        var result = await _accountService.FindByIdAsync(accountId);

        Assert.NotNull(result);
        Assert.Equal(cached.HolderName, result.HolderName);

        _accountRepositoryMock.Verify(r => r.FindByIdAsync(It.IsAny<Guid>()), Times.Never);
    }
}
