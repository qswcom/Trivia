using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Qsw.Module.Waiting.Interface
{
    public interface IWaitingService
    {
        Task<IList<string>> GetWaitingUserForRoom(long roomId);
        Task<IList<string>> GetUserIdsWithLessRoomWaitingList(int waitingNum, int pageSize);
        Task AddRoomUserInfo(IList<long> roomIds, string userId);
        Task AddRoomUserInfo(long roomId, IList<string> userIds);
        Task RemoveRoom(long roomId);
    }
}