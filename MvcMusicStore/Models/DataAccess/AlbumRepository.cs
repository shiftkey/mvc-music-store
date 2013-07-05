using System.Collections.Generic;
using System.Linq;

namespace MvcMusicStore.Models.DataAccess
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
        readonly IAlbumRepository repository;
        readonly ICacheService cacheService;

        public CacheableAlbumRepository(IAlbumRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        public List<Album> GetTopSellingAlbums(int count)
        {
            return cacheService.Get("top-selling-albums", () => repository.GetTopSellingAlbums(count));
        }
    }
}