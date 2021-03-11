using System.Threading.Tasks;

namespace Com.Qsw.Module.UserState.Interface
{
    public interface IUserStateInfoService
    {
        Task<UserStateInfo> GetOrCreate(string userId);
        Task<UserStateInfo> SetUserStateToWaiting(string userId);
        Task<UserStateInfo> SetUserStateToRoom(string userId, long roomId);
        Task<UserStateInfo> SetUserStateToGame(string userId, long gameId);
    }
}