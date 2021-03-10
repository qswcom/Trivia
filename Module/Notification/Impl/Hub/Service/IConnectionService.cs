namespace Com.Qsw.Module.Notification.Impl
{
    public interface IConnectionService
    {
        ConnectionContext GetConnectionContext(string userId);
    }
}