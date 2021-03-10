namespace Com.Qsw.Framework.Context.Web
{
    public class CallContextService : ICallContextService
    {
        public string ClientId => CallContext.GetData(CallContextConstants.ClientIdName) as string;

        public string UserId => CallContext.GetData(CallContextConstants.UserIdName) as string;
    }
}