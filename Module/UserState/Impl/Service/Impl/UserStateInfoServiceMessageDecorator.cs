using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    public class UserStateInfoServiceMessageDecorator : IUserStateInfoService
    {
        private readonly IUserStateInfoService decoratedService;
        private readonly IMessageService messageService;

        public UserStateInfoServiceMessageDecorator(IUserStateInfoService decoratedService,
            IMessageService messageService)
        {
            this.decoratedService = decoratedService;
            this.messageService = messageService;
        }

        public async Task<UserStateInfo> GetOrCreate(string userId)
        {
            return await decoratedService.GetOrCreate(userId);
        }

        public async Task<UserStateInfo> SetUserStateToWaiting(string userId)
        {
            UserStateInfo userStateInfo = await decoratedService.SetUserStateToWaiting(userId);
            await SendEntityChangedMessage(userStateInfo);
            return userStateInfo;
        }

        public async Task<UserStateInfo> SetUserStateToRoom(string userId, long roomId)
        {
            UserStateInfo userStateInfo = await decoratedService.SetUserStateToRoom(userId, roomId);
            await SendEntityChangedMessage(userStateInfo);
            return userStateInfo;
        }

        public async Task<UserStateInfo> SetUserStateToGame(string userId, long gameId)
        {
            UserStateInfo userStateInfo = await decoratedService.SetUserStateToGame(userId, gameId);
            await SendEntityChangedMessage(userStateInfo);
            return userStateInfo;
        }

        #region Message

        private async Task SendEntityChangedMessage(UserStateInfo userStateInfo)
        {
            await messageService.Publish(UserStateInfoChangedMessageTopic.Topic, null,
                new UserStateInfoChangedMessage(userStateInfo));
        }

        #endregion
    }
}