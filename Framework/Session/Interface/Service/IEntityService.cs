using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Qsw.Framework.Session.Interface
{
    public interface IEntityService
    {
    }

    public interface IEntityService<TEntity> : IEntityService
        where TEntity : IEntityId
    {
        Task<IList<TEntity>> LoadAll();
        Task<TEntity> Get(long id, bool updateLater = false);
        Task<TEntity> Save(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(long id);
    }
}