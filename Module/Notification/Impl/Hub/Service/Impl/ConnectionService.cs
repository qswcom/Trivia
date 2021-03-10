using System.Collections.Generic;

namespace Com.Qsw.Module.Notification.Impl
{
    public class ConnectionService : IConnectionManageService
    {
        private readonly IDictionary<string, ConnectionContext> connectionContextsByConnectionIdDic =
            new Dictionary<string, ConnectionContext>();
        private readonly IDictionary<string, ConnectionContext> connectionContextsByUserIdDic =
            new Dictionary<string, ConnectionContext>();

        private readonly object connectionContextsDicLock = new object();

        public void AddConnectionContext(ConnectionContext connectionContext)
        {
            if (connectionContext == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(connectionContext.ConnectionId))
            {
                return;
            }

            lock(connectionContextsDicLock)
            {
                connectionContextsByConnectionIdDic[connectionContext.ConnectionId] = connectionContext;
                connectionContextsByUserIdDic[connectionContext.UserId] = connectionContext;
            }
        }

        public void RemoveConnectionContext(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                return;
            }

            lock(connectionContextsDicLock)
            {
                if (connectionContextsByConnectionIdDic.TryGetValue(connectionId, out var connectionContext))
                {
                    connectionContextsByConnectionIdDic.Remove(connectionId);
                    connectionContextsByUserIdDic.Remove(connectionContext.UserId);
                }
            }
        }

        public ConnectionContext GetConnectionContext(string userId)
        {
            lock(connectionContextsDicLock)
            {
                connectionContextsByUserIdDic.TryGetValue(userId, out var connectionContext);
                return connectionContext;
            }
        }
    }
}