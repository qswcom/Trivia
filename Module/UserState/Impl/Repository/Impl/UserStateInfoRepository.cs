using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Impl;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    public class UserStateInfoRepository : EntityMemoryRepository<UserStateInfo>, IUserStateInfoRepository
    {
        private readonly IDictionary<string, long> idByUserIdDictionary;

        public UserStateInfoRepository()
        {
            idByUserIdDictionary = new Dictionary<string, long>();
        }

        public async Task<UserStateInfo> GetByUserId(string userId)
        {
            await Task.CompletedTask;
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            lock (this)
            {
                if (!idByUserIdDictionary.TryGetValue(userId, out long id))
                {
                    return default;
                }

                return Get(id).Result;
            }
        }

        public override async Task<UserStateInfo> Save(UserStateInfo entity)
        {
            if (entity == null)
            {
                return await base.Save(null);
            }

            if (string.IsNullOrWhiteSpace(entity.UserId))
            {
                throw new ArgumentNullException(nameof(entity.UserId));
            }

            lock (this)
            {
                UserStateInfo userStateInfo = base.Save(entity).Result;
                idByUserIdDictionary[userStateInfo.UserId] = userStateInfo.Id;
                return userStateInfo;
            }
        }

        public override async Task<UserStateInfo> Update(UserStateInfo entity)
        {
            if (entity == null)
            {
                return await base.Update(null);
            }

            if (string.IsNullOrWhiteSpace(entity.UserId))
            {
                throw new ArgumentNullException(nameof(entity.UserId));
            }

            lock (this)
            {
                UserStateInfo originalUserStateInfo = Get(entity.Id).Result;
                if (originalUserStateInfo == null)
                {
                    return base.Update(entity).Result;
                }

                if (originalUserStateInfo.UserId != entity.UserId)
                {
                    throw new ArgumentException(nameof(entity.UserId));
                }

                return base.Update(entity).Result;
            }
        }

        public override async Task Delete(long id)
        {
            await Task.CompletedTask;
            lock (this)
            {
                UserStateInfo originalUserStateInfo = Get(id).Result;
                if (originalUserStateInfo != null)
                {
                    idByUserIdDictionary.Remove(originalUserStateInfo.UserId);
                }

                base.Delete(id).Wait();
            }
        }
    }
}