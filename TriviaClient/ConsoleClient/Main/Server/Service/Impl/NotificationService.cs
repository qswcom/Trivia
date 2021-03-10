using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class NotificationService : INotificationService
    {
        private readonly IServerInfoService serverInfoService;
        
        private HubConnection hubConnection;

        public NotificationService(IServerInfoService serverInfoService)
        {
            this.serverInfoService = serverInfoService;
        }

        public Task ConnectToServer(string userName)
        {
            hubConnection =  new HubConnectionBuilder()
                .WithUrl($"{serverInfoService.ServerHost}/Hub/NotificationHub?user_id={userName}")
                .Build();
            
            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
                await hubConnection.StartAsync();
            };
            
            return hubConnection.StartAsync();
        }
        
        public void Dispose()
        {
            hubConnection.StopAsync().Wait();
        }
    }
}