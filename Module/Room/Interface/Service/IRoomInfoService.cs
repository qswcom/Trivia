using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Qsw.Module.Room.Interface
{
    public interface IRoomInfoService
    {
        Task<IList<RoomInfo>> LoadAll(int pageNum, int pageSize);
        Task<RoomInfo> Get(long roomId);
        Task<RoomInfo> CreateRoom(string userId);
        Task<RoomInfo> JoinRoom(long roomId, string userId);
        Task<RoomInfo> LeaveRoom(long roomId, string userId);
        Task<RoomInfo> DeleteRoom(long roomId);
        Task<RoomInfo> GetRoomAndDelete(string userId, long roomId);
    }
}