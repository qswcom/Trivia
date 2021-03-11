using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Qsw.Framework.Session.Interface
{
    public interface IEntityMemoryRepository<TEntity> : IEntityRepository
        where TEntity : IEntityId
    {
        IEnumerable<TEntity> All();
        Task<TEntity> Get(long id);
        Task<TEntity> Save(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(long id);
    }
}