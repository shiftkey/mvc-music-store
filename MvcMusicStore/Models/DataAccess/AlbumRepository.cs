using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace MvcMusicStore.Models.DataAccess
{
    public interface IAlbumRepository : IDisposable
    {
        List<Album> GetTopSellingAlbums(int count);
        Album GetById(int id);
        List<Album> GetAllSortedByPrice();
        void Insert(Album album);
        void Save();
        void Update(Album album);
        void Delete(int id);
    }

    public class AlbumRepository : IAlbumRepository
    {
        readonly MusicStoreEntities context = new MusicStoreEntities();

        public List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return context.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }

        public Album GetById(int id)
        {
            return context.Albums.Find(id);
        }

        public List<Album> GetAllSortedByPrice()
        {
            return context.Albums.Include(a => a.Genre)
                                 .Include(a => a.Artist)
                                 .OrderBy(a => a.Price)
                                 .ToList();
        }

        public void Insert(Album album)
        {
            context.Albums.Add(album);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Album album)
        {
            context.Entry(album).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var album = context.Albums.Find(id);
            context.Albums.Remove(album);
        }

        bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class AlbumRepositoryCache : IAlbumRepository
    {
        readonly IAlbumRepository repository;
        readonly ICacheService cacheService;

        public AlbumRepositoryCache(IAlbumRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        public List<Album> GetTopSellingAlbums(int count)
        {
            return cacheService.Get("top-selling-albums", () => repository.GetTopSellingAlbums(count));
        }

        public Album GetById(int id)
        {
            return cacheService.Get("album-" + id, () => repository.GetById(id));
        }

        public List<Album> GetAllSortedByPrice()
        {
            return cacheService.Get("all-sorted-by-price", () => repository.GetAllSortedByPrice());
        }

        public void Insert(Album album)
        {
            repository.Insert(album);
        }

        public void Save()
        {
            repository.Save();
        }

        public void Update(Album album)
        {
            repository.Update(album);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public void Dispose()
        {
            repository.Dispose();
        }
    }
}