using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Game.Interface;
using Com.Qsw.Module.Notification.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Com.Qsw.Module.Game.Impl
{
    public class NotificationWatchGameInfoChangedMonitor : INotificationWatchGameInfoChangedMonitor
    {
        private readonly ILogger logger;
        private readonly IMessageService messageService;
        private readonly INotificationService notificationService;

        public NotificationWatchGameInfoChangedMonitor(ILoggerFactory loggerFactory,
            IMessageService messageService, INotificationService notificationService)
        {
            logger = loggerFactory.CreateLogger<NotificationWatchGameInfoChangedMonitor>();
            this.messageService = messageService;
            this.notificationService = notificationService;
        }

        public void Start()
        {
            messageService.AddConsumer<GameInfoChangedMessage>(
                nameof(NotificationWatchGameInfoChangedMonitor),
                GameInfoChangedMessageTopic.Topic, OnGameChanged).Wait();
        }

        private Task OnGameChanged(string key, GameInfoChangedMessage gameInfoChangedMessage)
        {
            List<string> userIds = gameInfoChangedMessage.GameInfo.GameUserInfoByUserIdDictionary.Keys.ToList();
            GameInfoChangedMessage sendGameInfoChangedMessage = gameInfoChangedMessage.Clone();
            for (var i = 0; i < sendGameInfoChangedMessage.GameInfo.GameQuestionInfo.QuestionInfoList.Count; i++)
            {
                sendGameInfoChangedMessage.GameInfo.GameQuestionInfo.QuestionInfoList[i] = null;
            }

            foreach (string userId in userIds)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await notificationService.Notify(userId,
                            NotificationType.EntityChanged,
                            JsonConvert.SerializeObject(new EntityChangedNotificationData(nameof(GameInfo),
                                JsonConvert.SerializeObject(sendGameInfoChangedMessage))));
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