using FinancePlatform.API.Application.Interfaces.Cache;
using FinancePlatform.API.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FinancePlatform.API.Application.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _cacheSettings;

        public CacheService(IDistributedCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _cache = cache;
            _cacheSettings = cacheSettings.Value;
        }

        public async Task UpdateAccountListCacheAsync(Account account)
        {
            string accountListCacheKey = "accounts:list";

            // Tenta obter o cache da lista de contas
            var cachedAccounts = await GetAsync<List<Account>>(accountListCacheKey);

            // Se a lista estiver no cache, adiciona a nova conta
            if (cachedAccounts != null)
            {
                cachedAccounts.Add(account);
                await SetAsync(accountListCacheKey, cachedAccounts);
            }
            else
            {
                // Caso contrário, cria uma nova lista de contas e armazena
                var newAccountList = new List<Account> { account };
                await SetAsync(accountListCacheKey, newAccountList);
            }
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var cachedData = await _cache.GetStringAsync(key);

                if (cachedData == null)
                {
                    // Aqui você pode definir um valor padrão, vazio ou qualquer inicialização necessária.
                    var defaultValue = default(T);  // Isso cria um valor padrão do tipo genérico T
                    await SetAsync(key, defaultValue);  // Cria a chave no cache com o valor default
                    return defaultValue;
                }

                return JsonSerializer.Deserialize<T>(cachedData);
            }
            catch (Exception ex)
            {
                // Aqui você pode adicionar logging para identificar o tipo de erro.
                // Logar o erro pode te ajudar a diagnosticar se a falha foi de rede ou outra.
                Console.WriteLine($"Erro ao acessar o cache: {ex.Message}");
                // Opcionalmente, retornar um valor default caso ocorra uma exceção.
                return default(T);
            }
            }

        public async Task SetAsync<T>(string key, T data, TimeSpan? expiration = null)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null when setting cache.");
            }

            Console.WriteLine($"Tempo de expiração recebido: {expiration}");
            Console.WriteLine($"Tempo de expiração padrão configurado: {_cacheSettings.DefaultCacheExpirationMinutes}");

            var cacheExpiration = expiration ?? TimeSpan.FromMinutes(_cacheSettings.DefaultCacheExpirationMinutes);

            if (cacheExpiration <= TimeSpan.Zero)
            {
                Console.WriteLine("Corrigindo expiração para 1 minuto.");
                cacheExpiration = TimeSpan.FromMinutes(1);
            }

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheExpiration
            };

            var serializedData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(key, serializedData, options);
        }


        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task UpdateCacheIfNeededAsync<T>(string cacheKey, T entity)
        {
            var cachedEntity = await GetAsync<T>(cacheKey);

            if (cachedEntity != null)
            {
                await SetAsync(cacheKey, entity);
            }
        }

        public async Task RemoveFromCacheIfNeededAsync<T>(string cacheKey)
        {
            var cachedEntity = await GetAsync<T>(cacheKey);

            if (cachedEntity != null)
            {
                await RemoveAsync(cacheKey);
            }
        }
    }
}

