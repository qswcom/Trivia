using System;
using System.Threading.Tasks;

namespace Com.Qsw.Framework.Cache.Interface
{
    public interface ICacheService
    {
        Task<T> Set<T>(string key, T value, DateTimeOffset? absoluteExpiration = null);
        Task<T> Get<T>(string key);
        Task Delete(string key);
    }
}