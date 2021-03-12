using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class WaitingViewModel : BaseViewModel, IDisposable
    {
        private readonly IEntityChangedNotificationService entityChangedNotificationService;
        private readonly IWaitingService waitingService;

        private string error;
        private IReadOnlyDictionary<long, RoomInfo> roomInfoDic;

        public WaitingViewModel(IEntityChangedNotificationService entityChangedNotificationService,
            IWaitingService waitingService)
        {
            this.entityChangedNotificationService = entityChangedNotificationService;
            this.waitingService = waitingService;

            roomInfoDic = new Dictionary<long, RoomInfo>();
            
            this.entityChangedNotificationService.EntityChangedNotification += OnEntityChanged;
        }

        public string Error
        {
            get => error;
            set => SetValue(ref error, value);
        }

        public IReadOnlyDictionary<long, RoomInfo> RoomInfoDic
        {
            get => roomInfoDic;
            set => SetValue(ref roomInfoDic, value);
        }

        public void Dispose()
        {
            entityChangedNotificationService.EntityChangedNotification -= OnEntityChanged;
        }

        #region Bind

        public async Task Bind()
        {
            try
            {
                IList<RoomInfo> roomInfos = await waitingService.LoadAll();
                RoomInfoDic = roomInfos.ToDictionary(m => m.Id);
            }
            catch (Exception e)
            {
                Error = $"Error on bind waiting, error message: {e.Message}";
            }
        }

        #endregion
        
        #region Command
        
        public async void CreateRoomCommand()
        {
            try
            {
                await waitingService.CreateRoom();
            }
            catch (Exception e)
            {
                Error = $"Error on create room, error message: {e.Message}";
            }
        }

        public async void JoinRoomCommand()
        {
            try
            {
                Console.WriteLine("Please input room id.");
                string roomIdStr = Console.ReadLine();
                long roomId = long.Parse(roomIdStr??"");
                await waitingService.JoinRoom(roomId);
            }
            catch (Exception e)
            {
                Error = $"Error on join room, error message: {e.Message}";
            }
        }
        
        #endregion

        #region Notification

        private void OnEntityChanged(EntityChangedNotificationData entityChangedNotificationData)
        {
            try
            {
                if (!string.Equals(entityChangedNotificationData.EntityType, nameof(RoomInfo),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                var roomChangedMessage = JsonConvert.DeserializeObject<RoomChangedMessage>(
                    entityChangedNotificationData.EntityChangedMessageJson);
                if (roomChangedMessage.OperationType == OperationType.Save)
                {
                    Dictionary<long,RoomInfo> roomInfos = RoomInfoDic.ToDictionary(m => m.Key, m => m.Value);
                    roomInfos[roomChangedMessage.RoomInfo.Id] = roomChangedMessage.RoomInfo;
                    RoomInfoDic = roomInfos;
                }
                else if (roomChangedMessage.OperationType == OperationType.Update)
                {
                    Dictionary<long,RoomInfo> roomInfos = RoomInfoDic.ToDictionary(m => m.Key, m => m.Value);
                    roomInfos[roomChangedMessage.RoomInfo.Id] = roomChangedMessage.RoomInfo;
                    RoomInfoDic = roomInfos;
                }
                else if (roomChangedMessage.OperationType == OperationType.Delete)
                {
                    Dictionary<long,RoomInfo> roomInfos = RoomInfoDic.ToDictionary(m => m.Key, m => m.Value);
                    roomInfos.Remove(roomChangedMessage.RoomInfo.Id);
                    RoomInfoDic = roomInfos;
                }
            }
            catch (Exception e)
            {
                Error = $"Failed to bind waiting info, error message: {e.Message}";
            }
        }

        #endregion
    }
}