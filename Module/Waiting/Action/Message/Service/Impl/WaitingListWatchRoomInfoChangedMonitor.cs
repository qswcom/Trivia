using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Room.Interface;
using Com.Qsw.Module.Waiting.Interface;

namespace Com.Qsw.Module.Waiting.Action
{
    public class WaitingListWatchRoomInfoChangedMonitor : IWaitingListWatchRoomInfoChangedMonitor
    {
        private readonly IMessageService messageService;
        private readonly IWaitingService waitingService;
        private readonly IWaitingNotificationService waitingNotificationService;

        public WaitingListWatchRoomInfoChangedMonitor(IMessageService messageService,
            IWaitingService waitingService, IWaitingNotificationService waitingNotificationService)
        {
            this.messageService = messageService;
            this.waitingService = waitingService;
            this.waitingNotificationService = waitingNotificationService;
        }

        public void Start()
        {
            messageService.AddConsumer<RoomChangedMessage>(
                nameof(WaitingListWatchRoomInfoChangedMonitor),
                RoomChangedMessageTopic.Topic, OnRoomChanged).Wait();
        }

        private async Task OnRoomChanged(string key, RoomChangedMessage roomChangedMessage)
        {
            if (roomChangedMessage.OperationType == OperationType.Delete)
            {
                IList<string> userIds = await waitingService.GetWaitingUserForRoom(roomChangedMessage.RoomInfo.Id);
                await waitingService.RemoveRoom(roomChangedMessage.RoomInfo.Id);
                await waitingNotificationService.NotifyRoomChanged(roomChangedMessage, userIds);
                return;
            }

            if (roomChangedMessage.OperationType == OperationType.Save)
            {
                IList<string> userIds = await waitingService.GetUserIdsWithLessRoomWaitingList(
                    WaitingActionConstants.MaxShowWaitingRoomNum,
                    WaitingActionConstants.NewRoomNotificationUserMaxNum);

                await waitingService.AddRoomUserInfo(roomChangedMessage.RoomInfo.Id, userIds);
                await waitingNotificationService.NotifyRoomChanged(roomChangedMessage, userIds);
            }
        }
    }
}