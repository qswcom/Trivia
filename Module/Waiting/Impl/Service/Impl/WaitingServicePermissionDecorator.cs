using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Module.Waiting.Interface;

namespace Com.Qsw.Module.Waiting.Impl
{
    public class WaitingServicePermissionDecorator : IWaitingService
    {
        private readonly IWaitingService decoratedService;

        public WaitingServicePermissionDecorator(IWaitingService decoratedService)
        {
            this.decoratedService = decoratedService;
        }

        public Task<IList<string>> GetWaitingUserForRoom(long roomId)
        {
            return decoratedService.GetWaitingUserForRoom(roomId);
        }

        public Task<IList<string>> GetUserIdsWithLessRoomWaitingList(int waitingNum, int pageSize)
        {
            return decoratedService.GetUserIdsWithLessRoomWaitingList(waitingNum, pageSize);
        }

        public Task AddRoomUserInfo(IList<long> roomIds, string userId)
        {
            return decoratedService.AddRoomUserInfo(roomIds, userId);
        }

        public Task AddRoomUserInfo(long roomId, IList<string> userIds)
        {
            return decoratedService.AddRoomUserInfo(roomId, userIds);
        }

        public Task RemoveRoom(long roomId)
        {
            return decoratedService.RemoveRoom(roomId);
        }
    }
}