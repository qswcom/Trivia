using System;
using System.Threading.Tasks;
using Com.Qsw.Module.Notification.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Com.Qsw.Module.User.Interface
{
    public class NotificationHub : Hub
    {
        private readonly IConnectionManageService connectionManageService;

        public NotificationHub(IConnectionManageService connectionManageService)
        {
            this.connectionManageService = connectionManageService;
        }

        public override Task OnConnectedAsync()
        {
            ConnectionContext connectionContext = BuildConnectionContext(Context);
            connectionManageService.AddConnectionContext(connectionContext);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var contextConnectionId = Context.ConnectionId;
            connectionManageService.RemoveConnectionContext(contextConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        protected virtual ConnectionContext BuildConnectionContext(HubCallerContext context)
        {
            var connectionContext = new ConnectionContext {ConnectionId = context.ConnectionId};
            HttpContext httpContext = context.GetHttpContext();
            string userId = httpContext.Request.Query["user_id"];
            connectionContext.UserId = userId;
            return connectionContext;
        }
    }
}