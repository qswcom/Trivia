using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;
using NHibernate.Linq;

namespace Com.Qsw.Framework.Session.Impl
{
    public class EntityService<TEntity> : IEntityService<TEntity>
        where TEntity : IEntityId
    {
        private readonly IEntityRepository<TEntity> entityRepository;

        public EntityService(IEntityRepository<TEntity> entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        [Transaction(true)]
        public virtual async Task<IList<TEntity>> LoadAll()
        {
            using (SessionWrapper sessionWrapper = entityRepository.GetSessionWrapper(true))
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction(true))
                {
                    IQueryable<TEntity> entityQuery = entityRepository.GetQueryable(sessionWrapper);
                    List<TEntity> entities = await entityQuery.ToListAsync();
                    await transactionWrapper.Commit();
                    return entities;
                }
            }
        }

        [Transaction(IsolationLevel.RepeatableRead)]
        public virtual async Task<TEntity> Get(long id, bool updateLater = false)
        {
            using (SessionWrapper sessionWrapper = entityRepository.GetSessionWrapper(true))
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction(true))
                {
                    TEntity entity = await entityRepository.Get(id, updateLater);
                    await transactionWrapper.Commit();
                    return entity;
                }
            }
        }

        [Transaction(IsolationLevel.RepeatableRead)]
        public virtual async Task<TEntity> Save(TEntity entity)
        {
            using (SessionWrapper sessionWrapper = entityRepository.GetSessionWrapper())
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction())
                {
                    TEntity savedEntity = await entityRepository.Save(entity);
                    await transactionWrapper.Commit();
                    return savedEntity;
                }
            }
        }

        [Transaction(IsolationLevel.RepeatableRead)]
        public virtual async Task<TEntity> Update(TEntity entity)
        {
            using (SessionWrapper sessionWrapper = entityRepository.GetSessionWrapper())
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction())
                {
                    TEntity updatedEntity = await entityRepository.Update(entity);
                    await transactionWrapper.Commit();
                    return updatedEntity;
                }
            }
        }


        [Transaction(IsolationLevel.RepeatableRead)]
        public virtual async Task Delete(long id)
        {
            using (SessionWrapper sessionWrapper = entityRepository.GetSessionWrapper())
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction())
                {
                    await entityRepository.Delete(id);
                    await transactionWrapper.Commit();
                }
            }
        }
    }
}