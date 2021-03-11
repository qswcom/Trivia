using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class NotificationService : INotificationService
    {
        private readonly IServerInfoService serverInfoService;
        private readonly IUserInfoService userInfoService;
        private readonly IEntityChangedNotificationTriggerService entityChangedNotificationTriggerService;
        private readonly INotificationStatusChangedTriggerService notificationStatusChangedTriggerService;

        private HubConnection hubConnection;

        public NotificationService(IServerInfoService serverInfoService, IUserInfoService userInfoService,
            IEntityChangedNotificationTriggerService entityChangedNotificationTriggerService,
            INotificationStatusChangedTriggerService notificationStatusChangedTriggerService)
        {
            this.serverInfoService = serverInfoService;
            this.userInfoService = userInfoService;
            this.entityChangedNotificationTriggerService = entityChangedNotificationTriggerService;
            this.notificationStatusChangedTriggerService = notificationStatusChangedTriggerService;
        }

        public Task ConnectToServer()
        {
            UserInfo userInfo = userInfoService.UserInfo;
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{serverInfoService.ServerHost}/Hub/NotificationHub?user_id={userInfo.UserId}")
                .Build();

            hubConnection.On<string>("EntityChanged", entityChangedNotificationDataJson =>
            {
                var entityChangedNotificationData =
                    JsonConvert.DeserializeObject<EntityChangedNotificationData>(entityChangedNotificationDataJson);
                try
                {
                    entityChangedNotificationTriggerService.Trigger(entityChangedNotificationData);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error on send entity changed notification. Error message: {e.Message}.");
                }
            });

            hubConnection.Closed += async _ =>
            {
                notificationStatusChangedTriggerService.TriggerClosed();
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
                notificationStatusChangedTriggerService.TriggerReconnected();
            };

            return hubConnection.StartAsync();
        }

        public void Dispose()
        {
            hubConnection.StopAsync().Wait();
        }
    }
}