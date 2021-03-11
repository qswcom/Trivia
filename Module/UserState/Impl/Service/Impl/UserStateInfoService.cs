using System;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    public class UserStateInfoService : IUserStateInfoService
    {
        private readonly IUserStateInfoRepository userStateInfoRepository;
        private readonly IMessageService messageService;

        public UserStateInfoService(IUserStateInfoRepository userStateInfoRepository, IMessageService messageService)
        {
            this.userStateInfoRepository = userStateInfoRepository;
            this.messageService = messageService;
        }

        [Lock]
        public async Task<UserStateInfo> GetOrCreate(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            UserStateInfo userStateInfo = await userStateInfoRepository.GetByUserId(userId);
            if (userStateInfo != null)
            {
                return userStateInfo;
            }

            UserStateInfo newUserStateInfo = CreateNewUserStateInfo(userId);
            return await userStateInfoRepository.Save(newUserStateInfo);
        }

        [Lock]
        public async Task<UserStateInfo> SetUserStateToWaiting(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            UserStateInfo userStateInfo = await userStateInfoRepository.GetByUserId(userId);
            if (userStateInfo == null)
            {
                throw new ArgumentException(nameof(userId));
            }

            userStateInfo.UserState = Interface.UserState.Waiting;
            userStateInfo.RoomId = default;
            userStateInfo.GameId = default;
            UserStateInfo result = await userStateInfoRepository.Update(userStateInfo);

            await SendEntityChangedMessage(result);
            return result;
        }

        [Lock]
        public async Task<UserStateInfo> SetUserStateToRoom(string userId, long roomId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            UserStateInfo userStateInfo = await userStateInfoRepository.GetByUserId(userId);
            if (userStateInfo == null)
            {
                throw new ArgumentException(nameof(userId));
            }

            userStateInfo.UserState = Interface.UserState.Room;
            userStateInfo.RoomId = roomId;
            userStateInfo.GameId = default;
            UserStateInfo result = await userStateInfoRepository.Update(userStateInfo);

            await SendEntityChangedMessage(result);
            return result;
        }

        [Lock]
        public async Task<UserStateInfo> SetUserStateToGame(string userId, long gameId)
        {
            await Task.CompletedTask;
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (gameId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gameId));
            }

            UserStateInfo userStateInfo = await userStateInfoRepository.GetByUserId(userId);
            if (userStateInfo == null)
            {
                throw new ArgumentException(nameof(userId));
            }

            userStateInfo.UserState = Interface.UserState.Game;
            userStateInfo.RoomId = default;
            userStateInfo.GameId = gameId;
            UserStateInfo result = await userStateInfoRepository.Update(userStateInfo);

            await SendEntityChangedMessage(result);
            return result;
        }

        #region Message

        private async Task SendEntityChangedMessage(UserStateInfo userStateInfo)
        {
            await messageService.Publish(UserStateInfoChangedMessageTopic.Topic, null,
                new UserStateInfoChangedMessage(userStateInfo));
        }

        #endregion

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