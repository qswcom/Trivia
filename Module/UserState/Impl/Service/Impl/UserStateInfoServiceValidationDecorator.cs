using System;
using System.Threading.Tasks;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    public class UserStateInfoServiceValidationDecorator : IUserStateInfoService
    {
        private readonly IUserStateInfoService decoratedService;

        public UserStateInfoServiceValidationDecorator(IUserStateInfoService decoratedService)
        {
            this.decoratedService = decoratedService;
        }

        public Task<UserStateInfo> GetOrCreate(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return decoratedService.GetOrCreate(userId);
        }

        public Task<UserStateInfo> SetUserStateToWaiting(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return decoratedService.SetUserStateToWaiting(userId);
        }

        public Task<UserStateInfo> SetUserStateToRoom(string userId, long roomId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            return decoratedService.SetUserStateToRoom(userId, roomId);
        }

        public Task<UserStateInfo> SetUserStateToGame(string userId, long gameId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (gameId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gameId));
            }

            return decoratedService.SetUserStateToGame(userId, gameId);
        }
    }
}