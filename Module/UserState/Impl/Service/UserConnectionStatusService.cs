using System;
using System.Threading.Tasks;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    //TODO: realize later.
    public class UserConnectionStatusService : IUserConnectionStatusService
    {
        public event Action<UserConnectionStatus> UserDisconnectEvent;
        public event Action<UserConnectionStatus> UserConnectEvent;
        public Task<UserConnectionStatus> GetUserConnectionStatus(string userId)
        {
            return Task.FromResult(new UserConnectionStatus
            {
                UserId = userId,
                IsConnected = true
            });
        }
    }
}