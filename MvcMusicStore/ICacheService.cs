namespace MvcMusicStore
{
    public interface ICacheService
    {
        T Get<T>(int id);
        void Add<T>(int id, T entity);
    }
}
