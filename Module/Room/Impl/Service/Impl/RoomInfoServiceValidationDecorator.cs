using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Module.Room.Interface;

namespace Com.Qsw.Module.Room.Impl
{
    public class RoomInfoServiceValidationDecorator : IRoomInfoService
    {
        private readonly IRoomInfoService decoratedService;

        public RoomInfoServiceValidationDecorator(IRoomInfoService decoratedService)
        {
            this.decoratedService = decoratedService;
        }

        public Task<IList<RoomInfo>> LoadAll(int pageNum, int pageSize)
        {
            if (pageNum <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNum));
            }

            if (pageSize <= 0 || pageSize > RoomConstants.MaxRoomLoadAllNum)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            return decoratedService.LoadAll(pageNum, pageSize);
        }

        public Task<RoomInfo> Get(long roomId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            return decoratedService.Get(roomId);
        }

        public Task<RoomInfo> CreateRoom(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return decoratedService.CreateRoom(userId);
        }

        public Task<RoomInfo> JoinRoom(long roomId, string userId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return decoratedService.JoinRoom(roomId, userId);
        }

        public Task<RoomInfo> LeaveRoom(long roomId, string userId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return decoratedService.LeaveRoom(roomId, userId);
        }

        public Task<RoomInfo> DeleteRoom(long roomId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            return decoratedService.DeleteRoom(roomId);
        }
    }
}