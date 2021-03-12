using System.Threading.Tasks;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface IRoomService
    {
        Task<RoomInfo> Get(long roomId);
        Task LeaveRoom(long roomId);
        Task StartGame(long roomId);
    }
}