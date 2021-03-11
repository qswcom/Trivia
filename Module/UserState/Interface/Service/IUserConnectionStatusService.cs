using System;
using System.Threading.Tasks;

namespace Com.Qsw.Module.UserState.Interface
{
    public interface IUserConnectionStatusService
    {
        event Action<UserConnectionStatus> UserDisconnectEvent;
        event Action<UserConnectionStatus> UserConnectEvent;
        Task<UserConnectionStatus> GetUserConnectionStatus(string userId);

    }
}