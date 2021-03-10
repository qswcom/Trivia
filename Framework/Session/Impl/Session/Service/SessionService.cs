using Com.Qsw.Framework.Session.Interface;
using NHibernate;
using NHibernate.Context;

namespace Com.Qsw.Framework.Session.Impl
{
    public class SessionService : ISessionService
    {
        private readonly ISessionFactory sessionFactory;

        public SessionService(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public SessionWrapper GetCurrentSession(bool isReadOnly = false)
        {
            if (CurrentSessionContext.HasBind(sessionFactory))
            {
                ISession currentSession = sessionFactory.GetCurrentSession();
                return new SessionWrapper(sessionFactory, currentSession, false);
            }

            ISession session = sessionFactory.OpenSession().SessionWithOptions()
                .AutoClose().OpenSession();
            CurrentSessionContext.Bind(session);
            return new SessionWrapper(sessionFactory, session, true);
        }
    }
}