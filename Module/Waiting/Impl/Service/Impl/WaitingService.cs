using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Waiting.Interface;

namespace Com.Qsw.Module.Waiting.Impl
{
    public class WaitingService : IWaitingService
    {
        private readonly IDictionary<string, int> waitingNumByUserIdDictionary;
        private readonly IDictionary<long, ISet<string>> userIdSetByRoomIdDictionary;

        public WaitingService()
        {
            waitingNumByUserIdDictionary = new Dictionary<string, int>();
            userIdSetByRoomIdDictionary = new Dictionary<long, ISet<string>>();
        }

        [Lock]
        public Task<IList<string>> GetWaitingUserForRoom(long roomId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            IList<string> result = !userIdSetByRoomIdDictionary.TryGetValue(roomId, out ISet<string> userIdSet)
                ? new List<string>()
                : userIdSet.ToList();

            return Task.FromResult(result);
        }

        [Lock]
        public Task<IList<string>> GetUserIdsWithLessRoomWaitingList(int waitingNum, int pageSize)
        {
            if (waitingNum <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(waitingNum));
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            IList<string> result = waitingNumByUserIdDictionary.Where(m => m.Value < waitingNum).Select(m => m.Key)
                .Take(pageSize)
                .ToList();

            return Task.FromResult(result);
        }

        [Lock]
        public Task AddRoomUserInfo(IList<long> roomIds, string userId)
        {
            if (roomIds == null)
            {
                throw new ArgumentNullException(nameof(roomIds));
            }

            foreach (long roomId in roomIds)
            {
                if (roomId <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(roomIds));
                }
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            foreach (long roomId in roomIds)
            {
                if (!userIdSetByRoomIdDictionary.TryGetValue(roomId, out ISet<string> userIdSet))
                {
                    userIdSet = new HashSet<string>();
                    userIdSetByRoomIdDictionary[roomId] = userIdSet;
                }

                userIdSet.Add(userId);

                waitingNumByUserIdDictionary.TryGetValue(userId, out int waitingNum);
                waitingNumByUserIdDictionary[userId] = waitingNum + 1;
            }

            if (!waitingNumByUserIdDictionary.ContainsKey(userId))
            {
                waitingNumByUserIdDictionary[userId] = 0;
            }

            return Task.CompletedTask;
        }

        [Lock]
        public Task AddRoomUserInfo(long roomId, IList<string> userIds)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            if (userIds == null)
                                                             {
                                                                 throw new ArgumentNullException(nameof(userIds));
                                                             }
                                                 
                                                             foreach (string userId in userIds)
                                                             {
                                                                 if (string.IsNullOrWhiteSpace(userId))
                                                                 {
                                                                     throw new ArgumentNullException(nameof(userId));
                                                                 }
                                                             }
                                                 
                                                             foreach (string userId in userIds)
                                                             {
                                                                 if (!userIdSetByRoomIdDictionary.TryGetValue(roomId, out ISet<string> userIdSet))
                                                                 {
                                                                     userIdSet = new HashSet<string>();
                                                                     userIdSetByRoomIdDictionary[roomId] = userIdSet;
                                                                 }
                                                 
                                                                 userIdSet.Add(userId);
                                                 
                                                                 waitingNumByUserIdDictionary.TryGetValue(userId, out int waitingNum);
                waitingNumByUserIdDictionary[userId] = waitingNum + 1;
            }

            return Task.CompletedTask;
        }

        [Lock]
        public Task RemoveRoom(long roomId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            if (userIdSetByRoomIdDictionary.TryGetValue(roomId, out ISet<string> userIdSet))
            {
                userIdSetByRoomIdDictionary.Remove(roomId);
                foreach (string userId in userIdSet)
                {
                    int waitingNum = waitingNumByUserIdDictionary[userId];
                    waitingNum--;
                    if (waitingNum <= 0)
                    {
                        waitingNumByUserIdDictionary.Remove(userId);
                    }
                    else
                    {
                        waitingNumByUserIdDictionary[userId] = waitingNum;
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}