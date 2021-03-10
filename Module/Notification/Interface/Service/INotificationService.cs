using System.Threading;
using System.Threading.Tasks;

namespace Com.Qsw.Module.Notification.Interface
{
    public interface INotificationService
    {
        Task Notify(string userId, NotificationType notificationType, string notificationData,
            CancellationToken token = default);
    }
}