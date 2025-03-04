namespace FinancePlatform.API.Application.Interfaces.Cache
{
    public interface ICacheService
    {
        public Task<T?> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T data, TimeSpan? expiration = null);
        public Task RemoveAsync(string key);
        public Task UpdateCacheIfNeededAsync<T>(string cacheKey, T entity);
        public Task RemoveFromCacheIfNeededAsync<T>(string cacheKey);
    }
}
