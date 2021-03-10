using System.Security.Principal;
using System.Threading.Tasks;
using Com.Qsw.Framework.Context.Web;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Com.Qsw.TriviaServer.AppServer.Main
{
    internal class ContextFilter : IAsyncActionFilter, IOrderedFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IIdentity identity = context.HttpContext?.User?.Identity;
            if (identity != null && identity.IsAuthenticated)
            {
                string clientId = context.HttpContext.Request.Headers["client_id"];
                CallContext.SetData(CallContextConstants.ClientIdName, clientId);

                string userId = context.HttpContext.Request.Headers["user_id"];
                CallContext.SetData(CallContextConstants.UserIdName, userId);
            }

            await next();
        }

        public int Order { get; } = 1;
    }
}