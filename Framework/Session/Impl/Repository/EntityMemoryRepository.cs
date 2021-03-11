using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Framework.Session.Impl
{
    public class EntityMemoryRepository<TEntity> : IEntityMemoryRepository<TEntity>
        where TEntity : IEntityId
    {
        private readonly IDictionary<long, TEntity> entityByIdDictionary;
        private long nextAvailableId;

        public EntityMemoryRepository()
        {
            entityByIdDictionary = new Dictionary<long, TEntity>();
            nextAvailableId = 1;
        }

        public virtual async Task<TEntity> Get(long id)
        {
            await Task.CompletedTask;
            lock (this)
            {
                entityByIdDictionary.TryGetValue(id, out TEntity entity);
                return entity;
            }
        }

        public virtual async Task<TEntity> Save(TEntity entity)
        {
            await Task.CompletedTask;

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            lock (this)
            {
                entity.Id = nextAvailableId;
                nextAvailableId++;
                entityByIdDictionary[entity.Id] = entity;
                return entity;
            }
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            await Task.CompletedTask;

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            lock (this)
            {
                if (!entityByIdDictionary.ContainsKey(entity.Id))
                {
                    throw new ArgumentException(nameof(entity.Id));
                }

                entityByIdDictionary[entity.Id] = entity;
                return entity;
            }
        }

        public virtual async Task Delete(long id)
        {
            await Task.CompletedTask;
            lock (this)
            {
                entityByIdDictionary.Remove(id);
            }
        }
    }
}