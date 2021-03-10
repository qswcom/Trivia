using System.Threading;
using System.Threading.Tasks;
using Com.Qsw.Module.Notification.Interface;
using Com.Qsw.Module.User.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Com.Qsw.Module.Notification.Impl
{
    public class NotificationService : INotificationService
    {
        private readonly IConnectionService connectionService;
        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationService(IConnectionService connectionService, IHubContext<NotificationHub> hubContext)
        {
            this.connectionService = connectionService;
            this.hubContext = hubContext;
        }

        public async Task Notify(string userId, NotificationType notificationType, string notificationData,
            CancellationToken token = default)
        {
            ConnectionContext connectionContext = connectionService.GetConnectionContext(userId);
            if (connectionContext == null)
            {
                return;
            }

            string connectionId = connectionContext.ConnectionId;
            
            await hubContext.Clients.Client(connectionId)
                .SendAsync(notificationType.ToString(), notificationData, token);
        }
    }
}