using System.Collections.Generic;

namespace MvcMusicStore
{
    public interface ICacheService
    {
        T Get<T>(string key);
        void Add<T>(string key, T entity);
    }

    // TODO: handle cross-threading concerns
    // TODO: handle expiration concerns
    public class InMemoryCacheService : ICacheService
    {
        readonly IDictionary<string, object> storage = new Dictionary<string, object>();

        public T Get<T>(string key)
        {
            object value;
            if (storage.TryGetValue(key, out value))
                return (T)value;

            return default(T);
        }

        public void Add<T>(string key, T entity)
        {
            storage[key] = entity;
        }
    }
}