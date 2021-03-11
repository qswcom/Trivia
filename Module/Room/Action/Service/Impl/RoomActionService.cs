using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Module.Game.Interface;
using Com.Qsw.Module.Room.Interface;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.Room.Action
{
    public class RoomActionService : IRoomActionService
    {
        private readonly IRoomInfoService roomInfoService;
        private readonly IUserStateInfoService userStateInfoService;
        private readonly IGameInfoService gameInfoService;

        public RoomActionService(IRoomInfoService roomInfoService, IUserStateInfoService userStateInfoService,
            IGameInfoService gameInfoService)
        {
            this.roomInfoService = roomInfoService;
            this.userStateInfoService = userStateInfoService;
            this.gameInfoService = gameInfoService;
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

        public async Task StartGame(string userId, long roomId)
        {
            RoomInfo roomInfo = await roomInfoService.GetRoomAndDelete(userId, roomId);
            if (roomInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            List<string> userIds = roomInfo.RoomUserInfoByUserIdDictionary.Keys.ToList();
            GameInfo gameInfo = await gameInfoService.Create(userIds);
            foreach (string tempUserId in userIds)
            {
                await userStateInfoService.SetUserStateToGame(tempUserId, gameInfo.Id);
            }
        }
    }
}