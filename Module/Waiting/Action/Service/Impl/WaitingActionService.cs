using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Module.Room.Interface;
using Com.Qsw.Module.UserState.Interface;
using Com.Qsw.Module.Waiting.Interface;

namespace Com.Qsw.Module.Waiting.Action
{
    public class WaitingActionService : IWaitingActionService
    {
        private readonly IRoomInfoService roomInfoService;
        private readonly IWaitingService waitingService;
        private readonly IUserStateInfoService userStateInfoService;

        public WaitingActionService(IRoomInfoService roomInfoService, IWaitingService waitingService,
            IUserStateInfoService userStateInfoService)
        {
            this.roomInfoService = roomInfoService;
            this.waitingService = waitingService;
            this.userStateInfoService = userStateInfoService;
        }

        public async Task<IList<RoomInfo>> LoadAll(string userId)
        {
            IList<RoomInfo> roomInfos = await roomInfoService.LoadAll(1, WaitingActionConstants.MaxShowWaitingRoomNum);
            await waitingService.AddRoomUserInfo(roomInfos.Select(m => m.Id).ToList(), userId);
            return roomInfos;
        }

        public async Task CreateRoom(string userId)
        {
            RoomInfo roomInfo = await roomInfoService.CreateRoom(userId);
            await userStateInfoService.SetUserStateToRoom(userId, roomInfo.Id);
        }

        public async Task JoinRoom(string userId, long roomId)
        {
            RoomInfo roomInfo = await roomInfoService.JoinRoom(roomId, userId);
            await userStateInfoService.SetUserStateToRoom(userId, roomInfo.Id);
        }
    }
}