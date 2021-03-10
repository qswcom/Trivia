using System;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;
using NHibernate;
using NHibernate.Linq;

namespace Com.Qsw.Framework.Session.Impl
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : IEntityId
    {
        private readonly ISessionService sessionService;

        public EntityRepository(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        public SessionWrapper GetSessionWrapper(bool isReadOnly = false)
        {
            return sessionService.GetCurrentSession(isReadOnly);
        }

        public IQueryable<TEntity> GetQueryable(SessionWrapper sessionWrapper, bool updateLater = false)
        {
            if (sessionWrapper == null)
            {
                throw new ArgumentException("Session Wrapper can't be null.");
            }

            IQueryable<TEntity> entityQuery = sessionWrapper.Session.Query<TEntity>(GetTableEntityName());

            if (updateLater)
            {
                entityQuery = entityQuery.WithLock(LockMode.Upgrade);
            }

            return entityQuery;
        }

        public async Task<TEntity> Get(long id, bool updateLater = false)
        {
            if (id == 0)
            {
                return default;
            }

            using (SessionWrapper sessionWrapper = sessionService.GetCurrentSession(true))
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction(true))
                {
                    TEntity result = await GetQueryable(sessionWrapper, updateLater)
                        .FirstOrDefaultAsync(m => m.Id.Equals(id));
                    await transactionWrapper.Commit();
                    return result;
                }
            }
        }

        public async Task<TEntity> Save(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id != 0)
            {
                throw new ApplicationException("Entity id must be zero when saving.");
            }

            using (SessionWrapper sessionWrapper = sessionService.GetCurrentSession())
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction())
                {
                    ISession session = sessionWrapper.Session;

                    await session.SaveAsync(GetTableEntityName(), entity);
                    await session.FlushAsync();
                    await transactionWrapper.Commit();
                    return entity;
                }
            }
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id == 0)
            {
                throw new ArgumentNullException(nameof(entity), "Entity Id can't be null.");
            }

            using (SessionWrapper sessionWrapper = sessionService.GetCurrentSession())
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction())
                {
                    ISession session = sessionWrapper.Session;
                    object mergedObj = await session.MergeAsync(entity);
                    await session.FlushAsync();
                    await transactionWrapper.Commit();
                    return (TEntity) mergedObj;
                }
            }
        }

        public async Task Delete(long id)
        {
            if (id == 0)
            {
                return;
            }

            using (SessionWrapper sessionWrapper = sessionService.GetCurrentSession())
            {
                using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction())
                {
                    TEntity originalEntity = await Get(id, true);
                    if (originalEntity == null)
                    {
                        return;
                    }

                    ISession session = sessionWrapper.Session;
                    await session.DeleteAsync(GetTableEntityName(), originalEntity);
                    await session.FlushAsync();
                    await transactionWrapper.Commit();
                }
            }
        }

        #region Helper

        protected virtual string GetTableEntityName()
        {
            return $"{nameof(TEntity)}";
        }

        #endregion
    }
}