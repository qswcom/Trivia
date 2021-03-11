using System;
using System.Threading.Tasks;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    public class UserStateInfoServicePermissionDecorator : IUserStateInfoService
    {
        private readonly IUserStateInfoService decoratedService;

        public UserStateInfoServicePermissionDecorator(IUserStateInfoService decoratedService)
        {
            this.decoratedService = decoratedService;
        }

        public Task<UserStateInfo> GetOrCreate(string userId)
        {
            //TODO: Add permission check later.
            return decoratedService.GetOrCreate(userId);
        }

        public Task<UserStateInfo> SetUserStateToWaiting(string userId)
        {
            //TODO: Add permission check later.
            return decoratedService.SetUserStateToWaiting(userId);
        }

        public Task<UserStateInfo> SetUserStateToRoom(string userId, long roomId)
        {
            //TODO: Add permission check later.
            return decoratedService.SetUserStateToRoom(userId, roomId);
        }

        public Task<UserStateInfo> SetUserStateToGame(string userId, long gameId)
        {
            //TODO: Add permission check later.
            return decoratedService.SetUserStateToGame(userId, gameId);
        }
    }
}