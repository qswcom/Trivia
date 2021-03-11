using System;
using System.Threading.Tasks;
using Com.Qsw.Module.Room.Interface;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.Room.Action
{
    public class RoomActionService : IRoomActionService
    {
        private readonly IRoomInfoService roomInfoService;
        private readonly IUserStateInfoService userStateInfoService;

        public RoomActionService(IRoomInfoService roomInfoService, IUserStateInfoService userStateInfoService)
        {
            this.roomInfoService = roomInfoService;
            this.userStateInfoService = userStateInfoService;
        }

        public Task<RoomInfo> Get(long roomId)
        {
            return roomInfoService.Get(roomId);
        }

        public async Task LeaveRoom(string userId, long roomId)
        {
            await userStateInfoService.SetUserStateToWaiting(userId);
            await roomInfoService.LeaveRoom(roomId, userId);
        }

        public Task StartGame(string userId, long roomId)
        {
            throw new NotImplementedException();
        }
    }
}