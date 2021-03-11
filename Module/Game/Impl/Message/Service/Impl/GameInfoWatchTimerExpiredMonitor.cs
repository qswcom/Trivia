using System;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Module.Game.Interface;
using Com.Qsw.Module.Game.Timer;
using Microsoft.Extensions.Logging;

namespace Com.Qsw.Module.Game.Impl
{
    public class GameInfoWatchTimerExpiredMonitor : IGameInfoWatchTimerExpiredMonitor
    {
        private readonly ILogger logger;
        private readonly IMessageService messageService;
        private readonly IGameInfoService gameInfoService;

        public GameInfoWatchTimerExpiredMonitor(ILoggerFactory loggerFactory,
            IMessageService messageService, IGameInfoService gameInfoService)
        {
            logger = loggerFactory.CreateLogger<NotificationWatchGameInfoChangedMonitor>();
            this.messageService = messageService;
            this.gameInfoService = gameInfoService;
        }

        public void Start()
        {
            messageService.AddConsumer<GameTimeExpiredMessage>(
                nameof(GameInfoWatchTimerExpiredMonitor),
                GameTimeExpiredTopic.Topic, OnTimeExpired).Wait();
        }

        private async Task OnTimeExpired(string key, GameTimeExpiredMessage gameTimeExpiredMessage)
        {
            try
            {
                await gameInfoService.OnTimeExpired(gameTimeExpiredMessage.GameId, gameTimeExpiredMessage.UserId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on send time expired message.");
            }
        }
    }
}