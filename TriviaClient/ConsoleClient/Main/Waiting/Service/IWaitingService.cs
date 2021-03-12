using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface IWaitingService
    {
        Task<IList<RoomInfo>> LoadAll();
        Task CreateRoom();
        Task JoinRoom(long roomId);
    }
}