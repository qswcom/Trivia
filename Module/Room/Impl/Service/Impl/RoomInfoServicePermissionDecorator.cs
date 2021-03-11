using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Module.Room.Interface;

namespace Com.Qsw.Module.Room.Impl
{
    public class RoomInfoServicePermissionDecorator : IRoomInfoService
    {
        private readonly IRoomInfoService decoratedService;

        public RoomInfoServicePermissionDecorator(IRoomInfoService decoratedService)
        {
            //TODO: Add permission check later.
            this.decoratedService = decoratedService;
        }

        public Task<IList<RoomInfo>> LoadAll(int pageNum, int pageSize)
        {
            //TODO: Add permission check later.
            return decoratedService.LoadAll(pageNum, pageSize);
        }

        public Task<RoomInfo> Get(long roomId)
        {
            return decoratedService.Get(roomId);

        }

        public Task<RoomInfo> CreateRoom(string userId)
        {
            //TODO: Add permission check later.
            return decoratedService.CreateRoom(userId);
        }

        public Task<RoomInfo> JoinRoom(long roomId, string userId)
        {
            //TODO: Add permission check later.
            return decoratedService.JoinRoom(roomId, userId);
        }

        public Task<RoomInfo> LeaveRoom(long roomId, string userId)
        {
            //TODO: Add permission check later.
            return decoratedService.LeaveRoom(roomId, userId);
        }

        public Task<RoomInfo> DeleteRoom(long roomId)
        {
            //TODO: Add permission check later.
            return decoratedService.DeleteRoom(roomId);
        }
    }
}