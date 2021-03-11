using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Module.Notification.Interface;
using Com.Qsw.Module.Room.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Com.Qsw.Module.Waiting.Action
{
    public class WaitingNotificationService : IWaitingNotificationService
    {
        private readonly ILogger logger;
        private readonly INotificationService notificationService;

        public WaitingNotificationService(ILoggerFactory loggerFactory, INotificationService notificationService)
        {
            logger = loggerFactory.CreateLogger<WaitingNotificationService>();
            this.notificationService = notificationService;
        }

        public Task NotifyRoomChanged(RoomChangedMessage roomChangedMessage, IList<string> userIds)
        {
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