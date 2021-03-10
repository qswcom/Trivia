using System.Linq;
using System.Threading.Tasks;

namespace Com.Qsw.Framework.Session.Interface
{
    public interface IEntityRepository
    {
    }

    public interface IEntityRepository<TEntity> : IEntityRepository
        where TEntity : IEntityId
    {
        SessionWrapper GetSessionWrapper(bool isReadOnly = false);
        IQueryable<TEntity> GetQueryable(SessionWrapper sessionWrapper, bool updateLater = false);
        Task<TEntity> Get(long id, bool updateLater = false);
        Task<TEntity> Save(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(long id);
    }
}