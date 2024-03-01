namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemFunc, TimeSpan expirationTime);
        bool InvalidateCache(string key);
    }
}