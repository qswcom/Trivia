using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class UserStateInfoViewModel : BaseViewModel, IDisposable
    {
        private readonly IUserStateInfoService userStateInfoService;
        private readonly IEntityChangedNotificationService entityChangedNotificationService;
        private readonly IUserInfoService userInfoService;
        private UserState userState;
        private long roomId;
        private long gameId;
        private string error;

        public UserStateInfoViewModel(IUserStateInfoService userStateInfoService,
            IEntityChangedNotificationService entityChangedNotificationService, IUserInfoService userInfoService)
        {
            this.userStateInfoService = userStateInfoService;
            this.entityChangedNotificationService = entityChangedNotificationService;
            this.userInfoService = userInfoService;
            this.entityChangedNotificationService.EntityChangedNotification += OnEntityChanged;
        }

        public string Error
        {
            get => error;
            set => SetValue(ref error, value);
        }

        public UserState UserState
        {
            get => userState;
            set => SetValue(ref userState, value);
        }

        public long RoomId
        {
            get => roomId;
            set => SetValue(ref roomId, value);
        }

        public long GameId
        {
            get => gameId;
            set => SetValue(ref gameId, value);
        }

        public void Dispose()
        {
            entityChangedNotificationService.EntityChangedNotification -= OnEntityChanged;
        }

        #region Bind

        public async Task Bind()
        {
            UserStateInfo userStateInfo;
            try
            {
                userStateInfo = await userStateInfoService.GetOrCreate();
            }
            catch (Exception e)
            {
                Error = $"Error on get user state information, please input 'b' to try again. error message: {e.Message}";
                return;
            }
            await Bind(userStateInfo);
        }

        private async Task Bind(UserStateInfo userStateInfo)
        {
            await Task.CompletedTask;
            RoomId = userStateInfo.RoomId;
            GameId = userStateInfo.GameId;
            UserState = userStateInfo.UserState;
        }

        #endregion

        #region Notification

        private void OnEntityChanged(EntityChangedNotificationData entityChangedNotificationData)
        {
            try
            {
                if (!string.Equals(entityChangedNotificationData.EntityType, nameof(UserStateInfo),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                var userStateInfoChangedMessage = JsonConvert.DeserializeObject<UserStateInfoChangedMessage>(
                    entityChangedNotificationData
                        .EntityChangedMessageJson);
                if (userStateInfoChangedMessage.UserStateInfo.UserId == userInfoService.UserInfo.UserId)
                {
                    Bind(userStateInfoChangedMessage.UserStateInfo).Wait();
                }
            }
            catch (Exception e)
            {
                Error = $"Failed to bind user state info, error message: {e.Message}";
            }
        }

        #endregion
    }
}