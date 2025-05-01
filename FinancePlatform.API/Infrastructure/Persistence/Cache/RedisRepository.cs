using FinancePlatform.API.Application.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FinancePlatform.API.Infrastructure.Persistence.Cache
{
    public class RedisRepository : ICacheRepository
    {
        private readonly IDistributedCache _distributedCache;

        public RedisRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<T>> GetCollection<T>(string collectionKey)
        {
           var result = await _distributedCache.GetStringAsync(collectionKey);
            if (result == null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<IEnumerable<T>>(result);
        }

        public async Task<T> GetValue<T>(Guid id)
        {
            var key = id.ToString().ToLower();
            
            var result = await _distributedCache.GetStringAsync(key);
            if (result == null) 
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task SetCollection<T>(string collectionKey, IEnumerable<T> collection)
        {
            var keyCollection = collectionKey.ToString().ToLower();
            var newCollectionValue = JsonConvert.SerializeObject(collection);
            await _distributedCache.SetStringAsync(keyCollection, newCollectionValue);
        }

        public async Task SetValue<T>(Guid id, T obj)
        {
            var key = id.ToString().ToLower();
            var newValue = JsonConvert.SerializeObject(obj);
            await _distributedCache.SetStringAsync(key, newValue);
        }
    }
}
