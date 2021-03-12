using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class RoomViewModel : BaseViewModel, IDisposable
    {
        private readonly IEntityChangedNotificationService entityChangedNotificationService;
        private readonly IRoomService roomService;

        private string error;
        private RoomInfo roomInfo;

        public RoomViewModel(IEntityChangedNotificationService entityChangedNotificationService,
            IRoomService roomService)
        {
            this.entityChangedNotificationService = entityChangedNotificationService;
            this.roomService = roomService;

            this.entityChangedNotificationService.EntityChangedNotification += OnEntityChanged;
        }

        public string Error
        {
            get => error;
            set => SetValue(ref error, value);
        }

        public RoomInfo RoomInfo
        {
            get => roomInfo;
            set => SetValue(ref roomInfo, value);
        }

        public void Dispose()
        {
            entityChangedNotificationService.EntityChangedNotification -= OnEntityChanged;
        }

        #region Bind

        public async Task Bind(long roomId)
        {
            try
            {
                RoomInfo = await roomService.Get(roomId);
            }
            catch (Exception e)
            {
                Error = $"Error on bind room, error message: {e.Message}";
            }
        }

        #endregion
        
        #region Command

        public async void LeftRoomCommand()
        {
            try
            {
                await roomService.LeaveRoom(roomInfo?.Id ?? 0);
            }
            catch (Exception e)
            {
                Error = $"Error on left room, error message: {e.Message}";
            }
        }

        public async void StartGameCommand()
        {
            try
            {
                if (RoomInfo == null)
                {
                    Error = $"Can't find joined room.";
                }
                else
                {
                    await roomService.StartGame(roomInfo.Id);
                }
            }
            catch (Exception e)
            {
                Error = $"Error on start game, error message: {e.Message}";
            }
        }
        
        #endregion

        #region Notification

        private void OnEntityChanged(EntityChangedNotificationData entityChangedNotificationData)
        {
            try
            {
                if (RoomInfo == null)
                {
                    return;
                }

                if (!string.Equals(entityChangedNotificationData.EntityType, nameof(RoomInfo),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                var roomChangedMessage = JsonConvert.DeserializeObject<RoomChangedMessage>(
                    entityChangedNotificationData.EntityChangedMessageJson);

                if (RoomInfo.Id != roomChangedMessage.RoomInfo.Id)
                {
                    return;
                }

                if (roomChangedMessage.OperationType == OperationType.Update)
                {
                    RoomInfo = roomChangedMessage.RoomInfo;
                }
                else if (roomChangedMessage.OperationType == OperationType.Delete)
                {
                    RoomInfo = null;
                }
            }
            catch (Exception e)
            {
                Error = $"Failed to bind room info, error message: {e.Message}";
            }
        }

        #endregion
    }
}