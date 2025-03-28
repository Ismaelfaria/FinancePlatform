using FinancePlatform.API.Application.Interfaces.Cache;
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
            // Criar a chave automaticamente caso ela não exista
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null when setting cache.");
            }

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(_cacheSettings.DefaultCacheExpirationMinutes)
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

