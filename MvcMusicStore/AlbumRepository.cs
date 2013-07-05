using System.Collections.Generic;
using System.Linq;
using MvcMusicStore.Models;

namespace MvcMusicStore
{
    public interface IAlbumRepository
    {
        List<Album> GetTopSellingAlbums(int count);
    }

    public class AlbumRepository : IAlbumRepository
    {
        readonly MusicStoreEntities storeDB = new MusicStoreEntities();

        public List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }

    public class CacheableAlbumRepository : IAlbumRepository
    {
        readonly IAlbumRepository _inner;
        readonly ICacheService _cacheService;

        public CacheableAlbumRepository(IAlbumRepository inner, ICacheService cacheService)
        {
            _inner = inner;
            _cacheService = cacheService;
        }

        public List<Album> GetTopSellingAlbums(int count)
        {
            var cachedValue = _cacheService.Get<List<Album>>("top-selling-albums");
            if (cachedValue == default(List<Album>))
            {
                var freshValue = _inner.GetTopSellingAlbums(count);
                _cacheService.Add("top-selling-albums", freshValue);
                return freshValue;
            }
            
            return cachedValue;
        }
    }
}