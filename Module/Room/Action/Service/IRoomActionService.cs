using System.Threading.Tasks;
using Com.Qsw.Module.Room.Interface;

namespace Com.Qsw.Module.Room.Action
{
    public interface IRoomActionService
    {
        Task<RoomInfo> Get(long roomId);
        Task LeaveRoom(string userId, long roomId);
        Task StartGame(string userId, long roomId);
    }
}