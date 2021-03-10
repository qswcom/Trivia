
namespace Com.Qsw.Module.Notification.Impl
{
    public interface IConnectionManageService : IConnectionService
    {
        void AddConnectionContext(ConnectionContext connectionContext);
        void RemoveConnectionContext(string connectionId);
    }
}