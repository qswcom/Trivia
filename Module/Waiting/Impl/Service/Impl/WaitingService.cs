using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<IList<string>> GetWaitingUserForRoom(long roomId)
        {
            IList<string> result;
            lock (this)
            {
                if (!userIdSetByRoomIdDictionary.TryGetValue(roomId, out ISet<string> userIdSet))
                {
                    result = new List<string>();
                }
                else
                {
                    result = userIdSet.ToList();
                }
            }

            return Task.FromResult(result);
        }

        public Task<IList<string>> GetUserIdsWithLessRoomWaitingList(int waitingNum, int pageSize)
        {
            IList<string> result;
            lock (this)
            {
                result = waitingNumByUserIdDictionary.Where(m => m.Value < waitingNum).Select(m => m.Key).Take(pageSize)
                    .ToList();
            }

            return Task.FromResult(result);
        }

        public Task AddRoomUserInfo(IList<long> roomIds, string userId)
        {
            lock (this)
            {
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
            }

            return Task.CompletedTask;
        }

        public Task AddRoomUserInfo(long roomId, IList<string> userIds)
        {
            lock (this)
            {
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
            }

            return Task.CompletedTask;
        }

        public Task RemoveRoom(long roomId)
        {
            lock (this)
            {
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
            }

            return Task.CompletedTask;
        }
    }
}