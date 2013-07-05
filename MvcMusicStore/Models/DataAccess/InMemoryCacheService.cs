using System;
using System.Collections.Generic;

namespace MvcMusicStore.Models.DataAccess
{
    public interface ICacheService
    {
        T Get<T>(string key);
        T Get<T>(string key, Func<T> produceResult);
    }

    // TODO: handle cross-threading concerns
    // TODO: handle expiration concerns
    public class InMemoryCacheService : ICacheService
    {
        readonly IDictionary<string, object> storage = new Dictionary<string, object>();

        public T Get<T>(string key)
        {
            return Get(key, () => default(T));
        }

        public T Get<T>(string key, Func<T> produceResult)
        {
            object value;
            if (storage.TryGetValue(key, out value))
                return (T)value;

            var newValue = produceResult();

            if (!Equals(newValue, default(T)))
            {
                storage[key] = newValue;
            }

            return newValue;
        }
    }
}