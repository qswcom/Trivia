using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class GameViewModel : BaseViewModel, IDisposable
    {
        private readonly IEntityChangedNotificationService entityChangedNotificationService;
        private readonly IGameService gameService;
        private readonly IUserInfoService userInfoService;

        private string error;
        private GameInfo gameInfo;

        public GameViewModel(IEntityChangedNotificationService entityChangedNotificationService,
            IGameService gameService, IUserInfoService userInfoService)
        {
            this.entityChangedNotificationService = entityChangedNotificationService;
            this.gameService = gameService;
            this.userInfoService = userInfoService;
            this.entityChangedNotificationService.EntityChangedNotification += OnEntityChanged;
        }
        
        public string Error
        {
            get => error;
            set => SetValue(ref error, value);
        }

        public GameInfo GameInfo
        {
            get => gameInfo;
            set => SetValue(ref gameInfo, value);
        }

        public UserInfo UserInfo => userInfoService.UserInfo;

        public void Dispose()
        {
            entityChangedNotificationService.EntityChangedNotification -= OnEntityChanged;
        }
        
        #region Bind

        public async Task Bind(long gameId)
        {
            try
            {
                GameInfo = await gameService.RetrieveQuestion(gameId);
            }
            catch (Exception e)
            {
                Error = $"Error on bind game, error message: {e.Message}";
            }
        }

        #endregion
        
        #region Command

        public async void SubmitAnswerCommand()
        {
            try
            {
                if (GameInfo == null)
                {
                    Error = $"Can't find game info.";
                }
                else
                {
                    Console.WriteLine("Please input answer.");
                    string answer = Console.ReadLine();
                    await gameService.SubmitAnswer(GameInfo.Id, answer);
                }
            }
            catch (Exception e)
            {
                Error = $"Error on submit answer, error message: {e.Message}";
            }
        }

        public async void LeaveGameCommand()
        {
            try
            {
                if (GameInfo == null)
                {
                    Error = $"Can't find game info.";
                }
                else
                {
                    await gameService.LeftGame(gameInfo.Id);
                }
            }
            catch (Exception e)
            {
                Error = $"Error on left room, error message: {e.Message}";
            }
        }
        
        #endregion
        
        #region Notification

        private void OnEntityChanged(EntityChangedNotificationData entityChangedNotificationData)
        {
            try
            {
                if (GameInfo == null)
                {
                    return;
                }

                if (!string.Equals(entityChangedNotificationData.EntityType, nameof(GameInfo),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                var gameInfoChangedMessage = JsonConvert.DeserializeObject<GameInfoChangedMessage>(
                    entityChangedNotificationData.EntityChangedMessageJson);

                if (gameInfo.Id != gameInfoChangedMessage.GameInfo.Id)
                {
                    return;
                }

                if (gameInfoChangedMessage.OperationType == OperationType.Update)
                {
                    Bind(gameInfoChangedMessage.GameInfo.Id).Wait();
                }
            }
            catch (Exception e)
            {
                Error = $"Failed to bind game info, error message: {e.Message}";
            }
        }

        #endregion
        
    }
}