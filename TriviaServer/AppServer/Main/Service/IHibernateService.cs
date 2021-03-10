using NHibernate;

namespace Com.Qsw.TriviaServer.AppServer.Main
{
    public interface IHibernateService
    {
        void InitHibernate();
        ISessionFactory GetSessionFactory();
    }
}