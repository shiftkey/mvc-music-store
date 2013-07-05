using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcMusicStore.Models.DataAccess
{
    public interface IGenresRepository : IDisposable
    {
        List<Genre> GetAll();
        List<Genre> GetSorted(int take);
        Genre Get(string genre);
    }

    public class GenresRepository : IGenresRepository
    {
        readonly MusicStoreEntities context = new MusicStoreEntities();

        public List<Genre> GetAll()
        {
            return context.Genres.Include("Albums").ToList();
        }

        public List<Genre> GetSorted(int take)
        {
            return context.Genres.OrderByDescending(
                g => g.Albums.Sum(
                    a => a.OrderDetails.Sum(
                        od => od.Quantity)))
                .Take(9)
                .ToList();
        }

        public Genre Get(string genre)
        {
            return context.Genres.Include("Albums").Single(g => g.Name == genre);
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

    public class GenresRepositoryCache : IGenresRepository
    {
        readonly IGenresRepository repository;
        readonly ICacheService cacheService;

        public GenresRepositoryCache(IGenresRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        public List<Genre> GetAll()
        {
            return cacheService.Get("genres-getall", () => repository.GetAll());
        }

        public List<Genre> GetSorted(int take)
        {
            return cacheService.Get("genres-getsorted-" + take, () => repository.GetSorted(take));
        }

        public Genre Get(string genre)
        {
            return cacheService.Get("genres-get" + genre, () => repository.Get(genre));
        }
    }
}