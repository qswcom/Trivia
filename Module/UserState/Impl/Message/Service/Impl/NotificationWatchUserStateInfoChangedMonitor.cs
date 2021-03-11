using System;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Module.Notification.Interface;
using Com.Qsw.Module.UserState.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Com.Qsw.Module.UserState.Impl
{
    public class NotificationWatchUserStateInfoChangedMonitor : INotificationWatchUserStateInfoChangedMonitor
    {
        private readonly ILogger logger;
        private readonly IMessageService messageService;
        private readonly INotificationService notificationService;

        public NotificationWatchUserStateInfoChangedMonitor(ILoggerFactory loggerFactory,
            IMessageService messageService, INotificationService notificationService)
        {
            logger = loggerFactory.CreateLogger<NotificationWatchUserStateInfoChangedMonitor>();
            this.messageService = messageService;
            this.notificationService = notificationService;
        }

        public void Start()
        {
            messageService.AddConsumer<UserStateInfoChangedMessage>(
                nameof(NotificationWatchUserStateInfoChangedMonitor),
                UserStateInfoChangedMessageTopic.Topic, OnUserStateInfoChanged).Wait();
        }

        private Task OnUserStateInfoChanged(string key, UserStateInfoChangedMessage userStateInfoChangedMessage)
        {
            Task.Run(async () =>
            {
                try
                {
                    await notificationService.Notify(userStateInfoChangedMessage.UserStateInfo.UserId,
                        NotificationType.EntityChanged,
                        JsonConvert.SerializeObject(new EntityChangedNotificationData(nameof(UserStateInfo),
                            JsonConvert.SerializeObject(userStateInfoChangedMessage))));
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error on send notification to user.");
                }
            });
            return Task.CompletedTask;
        }
    }
}