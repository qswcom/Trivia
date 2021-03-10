namespace Com.Qsw.Framework.Session.Interface
{
    public interface ISessionService
    {
        SessionWrapper GetCurrentSession(bool isReadOnly = false);
    }
}