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
}