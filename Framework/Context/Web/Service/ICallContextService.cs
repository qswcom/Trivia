namespace Com.Qsw.Framework.Context.Web
{
    public interface ICallContextService
    {
        public string ClientId { get; }
        public string UserId { get; }
    }
}