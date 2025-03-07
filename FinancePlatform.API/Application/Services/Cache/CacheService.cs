﻿using FinancePlatform.API.Application.Interfaces.Cache;
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
            var cachedData = await _cache.GetStringAsync(key);
            return cachedData == null ? default : JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(string key, T data, TimeSpan? expiration = null)
        {
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

