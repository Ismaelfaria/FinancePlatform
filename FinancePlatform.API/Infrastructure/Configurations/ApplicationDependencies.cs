using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Application.Interfaces.Repositories;
using FinancePlatform.API.Application.Interfaces.Services;
using FinancePlatform.API.Application.Interfaces.UseCases;
using FinancePlatform.API.Application.Interfaces.Utils;
using FinancePlatform.API.Application.Interfaces.Validator;
using FinancePlatform.API.Application.Services;
using FinancePlatform.API.Application.UseCases;
using FinancePlatform.API.Application.Utils;
using FinancePlatform.API.Application.Validators;
using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Infrastructure.Persistence.Repositories;
using FluentValidation;
using FinancePlatform.API.Infrastructure.Persistence.Cache;

namespace FinancePlatform.API.Infrastructure.Configurations
{
    public static class ApplicationDependencies
    {
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            // Serviços
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IReconciliationService, ReconciliationService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<INotificationService, NotificationService>();

            // Use Cases
            services.AddScoped<IAccountUseCase, AccountUseCase>();
            services.AddScoped<IPaymentUseCase, PaymentUseCase>();

            // Repositórios
            services.AddScoped<IReconciliationRepository, ReconciliationRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            // Estratégias de Atualização
            services.AddScoped<IEntityUpdateStrategy, EntityUpdateStrategy>();

            // Validators
            services.AddTransient<IValidator<Account>, ValidatorAccount>();
            services.AddTransient<IValidator<Guid>, ValidatorGuid>();
            services.AddTransient<IValidator<Reconciliation>, ValidatorReconciliation>();
            services.AddTransient<IValidator<Payment>, ValidatorPayment>();
            services.AddTransient<IValidator<Notification>, ValidatorNotification>();
            services.AddTransient<IValidatorDebitAndWithdraw, ValidatorDebitAndWithdraw>();

            // Cache
            services.AddScoped<ICacheRepository, RedisRepository>();

            services.AddValidatorsFromAssemblyContaining<Program>();
        }
    }
}
