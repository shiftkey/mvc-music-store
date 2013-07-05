using System.Collections.Generic;

namespace MvcMusicStore
{
    // TODO: handle cross-threading concerns
    // TODO: handle expiration concerns
    public class InMemoryCacheService : ICacheService
    {
        readonly IDictionary<string, object> storage = new Dictionary<string, object>();

        public T Get<T>(int id)
        {
            var key = GetKey<T>(id);

            object value;
            if (storage.TryGetValue(key, out value))
            {
                return (T) value;
            }

            return default(T);
        }

        private static string GetKey<T>(int id)
        {
            var key = string.Format("{0}-{1}", typeof (T).FullName, id);
            return key;
        }

        public void Add<T>(int id, T entity)
        {
            var key = GetKey<T>(id);

            storage[key] = entity;
        }
    }
}