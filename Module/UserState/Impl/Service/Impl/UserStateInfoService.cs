using System;
using System.Threading.Tasks;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    public class UserStateInfoService : IUserStateInfoService
    {
        private readonly IUserStateInfoRepository userStateInfoRepository;

        public UserStateInfoService(IUserStateInfoRepository userStateInfoRepository)
        {
            this.userStateInfoRepository = userStateInfoRepository;
        }

        public async Task<UserStateInfo> GetOrCreate(string userId)
        {
            await Task.CompletedTask;
            lock (this)
            {
                UserStateInfo userStateInfo = userStateInfoRepository.GetByUserId(userId).Result;
                if (userStateInfo != null)
                {
                    return userStateInfo;
                }

                UserStateInfo newUserStateInfo = CreateNewUserStateInfo(userId);
                return userStateInfoRepository.Save(newUserStateInfo).Result;
            }
        }

        public async Task<UserStateInfo> SetUserStateToWaiting(string userId)
        {
            await Task.CompletedTask;
            lock (this)
            {
                UserStateInfo userStateInfo = userStateInfoRepository.GetByUserId(userId).Result;
                if (userStateInfo == null)
                {
                    throw new ArgumentException(nameof(userId));
                }

                userStateInfo.UserState = Interface.UserState.Waiting;
                userStateInfo.RoomId = default;
                userStateInfo.GameId = default;
                return userStateInfoRepository.Update(userStateInfo).Result;
            }
        }

        public async Task<UserStateInfo> SetUserStateToRoom(string userId, long roomId)
        {
            await Task.CompletedTask;
            lock (this)
            {
                UserStateInfo userStateInfo = userStateInfoRepository.GetByUserId(userId).Result;
                if (userStateInfo == null)
                {
                    throw new ArgumentException(nameof(userId));
                }

                userStateInfo.UserState = Interface.UserState.Room;
                userStateInfo.RoomId = roomId;
                userStateInfo.GameId = default;
                return userStateInfoRepository.Update(userStateInfo).Result;
            }
        }

        public async Task<UserStateInfo> SetUserStateToGame(string userId, long gameId)
        {
            await Task.CompletedTask;
            lock (this)
            {
                UserStateInfo userStateInfo = userStateInfoRepository.GetByUserId(userId).Result;
                if (userStateInfo == null)
                {
                    throw new ArgumentException(nameof(userId));
                }

                userStateInfo.UserState = Interface.UserState.Game;
                userStateInfo.RoomId = default;
                userStateInfo.GameId = gameId;
                return userStateInfoRepository.Update(userStateInfo).Result;
            }
        }

        #region Helper

        private UserStateInfo CreateNewUserStateInfo(string userId)
        {
            return new UserStateInfo
            {
                UserId = userId,
                UserState = Interface.UserState.Waiting
            };
        }

        #endregion
    }
}