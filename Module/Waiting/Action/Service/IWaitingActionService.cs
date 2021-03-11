using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Module.Room.Interface;

namespace Com.Qsw.Module.Waiting.Action
{
    public interface IWaitingActionService
    {
        Task<IList<RoomInfo>> LoadAll(string userId);
        Task CreateRoom(string userId);
        Task JoinRoom(string userId, long roomId);
    }
}