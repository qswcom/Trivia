using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Module.Waiting.Interface;

namespace Com.Qsw.Module.Waiting.Impl
{
    public class WaitingServiceValidationDecorator : IWaitingService
    {
        private readonly IWaitingService decoratedService;

        public WaitingServiceValidationDecorator(IWaitingService decoratedService)
        {
            this.decoratedService = decoratedService;
        }

        public Task<IList<string>> GetWaitingUserForRoom(long roomId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            return decoratedService.GetWaitingUserForRoom(roomId);
        }

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

            return decoratedService.GetUserIdsWithLessRoomWaitingList(waitingNum, pageSize);
        }

        public Task AddRoomUserInfo(IList<long> roomIds, string userId)
        {
            if (roomIds == null || roomIds.Count < 0)
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

            return decoratedService.AddRoomUserInfo(roomIds, userId);
        }

        public Task AddRoomUserInfo(long roomId, IList<string> userIds)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            if (userIds == null || userIds.Count < 0)
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

            return decoratedService.AddRoomUserInfo(roomId, userIds);
        }

        public Task RemoveRoom(long roomId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            return decoratedService.RemoveRoom(roomId);
        }
    }
}