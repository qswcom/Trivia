using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Module.Notification.Interface;
using Com.Qsw.Module.Room.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Com.Qsw.Module.Room.Impl
{
    public class NotificationWatchRoomInfoChangedMonitor : INotificationWatchRoomInfoChangedMonitor
    {
        private readonly ILogger logger;
        private readonly IMessageService messageService;
        private readonly INotificationService notificationService;

        public NotificationWatchRoomInfoChangedMonitor(ILoggerFactory loggerFactory,
            IMessageService messageService, INotificationService notificationService)
        {
            logger = loggerFactory.CreateLogger<NotificationWatchRoomInfoChangedMonitor>();
            this.messageService = messageService;
            this.notificationService = notificationService;
        }

        public void Start()
        {
            messageService.AddConsumer<RoomChangedMessage>(
                nameof(NotificationWatchRoomInfoChangedMonitor),
                RoomChangedMessageTopic.Topic, OnRoomChanged).Wait();
        }

        private Task OnRoomChanged(string key, RoomChangedMessage roomChangedMessage)
        {
            List<string> userIds = roomChangedMessage.RoomInfo.RoomUserInfoByUserIdDictionary.Keys.ToList();
            foreach (string userId in userIds)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await notificationService.Notify(userId,
                            NotificationType.EntityChanged,
                            JsonConvert.SerializeObject(new EntityChangedNotificationData(nameof(RoomInfo),
                                JsonConvert.SerializeObject(roomChangedMessage))));
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error on send notification to user.");
                    }
                });
            }

            return Task.CompletedTask;
        }
    }
}