using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcMusicStore.Models.DataAccess
{
    public interface IArtistsRepository : IDisposable
    {
        IEnumerable<Artist> GetAll();
    }
    
    public class ArtistsRepository : IArtistsRepository
    {
        readonly MusicStoreEntities context = new MusicStoreEntities();

        public IEnumerable<Artist> GetAll()
        {
            return context.Artists.ToList();
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

    public class ArtistsRepositoryCache : IArtistsRepository
    {
        readonly IArtistsRepository repository;
        readonly ICacheService cacheService;

        public ArtistsRepositoryCache(IArtistsRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        public IEnumerable<Artist> GetAll()
        {
            return cacheService.Get("artists-all", () => repository.GetAll());
        }
    }
}