using System;
using System.Data;
using NHibernate;
using NHibernate.Context;

namespace Com.Qsw.Framework.Session.Interface
{
    public class SessionWrapper : IDisposable
    {
        public SessionWrapper(ISessionFactory sessionFactory, ISession session, bool isCreator)
        {
            SessionFactory = sessionFactory;
            Session = session;
            IsCreator = isCreator;
        }
        
        public ISession Session { get; }
        private bool IsCreator { get; }
        private ISessionFactory SessionFactory { get; }

        public TransactionWrapper BuildTransaction(bool isReadOnly = false,
            IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
        {
            if (isolationLevel == IsolationLevel.Chaos)
            {
                throw new ArgumentOutOfRangeException(nameof(isolationLevel),
                    "The IsolationLevel Chaos(16), is not supported by the .Net Framework SqlClient Data Provider.");
            }

            ISession session = Session;

            ITransaction transaction = session.GetCurrentTransaction();
            if (transaction == null || !transaction.IsActive)
            {
                if (isReadOnly)
                {
                    session.FlushMode = FlushMode.Manual;
                }

                transaction = session.BeginTransaction(isolationLevel);
                return new TransactionWrapper(this, transaction, true);
            }

            return new TransactionWrapper(this, transaction, false);
        }

        public void Dispose()
        {
            if (!IsCreator)
            {
                return;
            }

            if (CurrentSessionContext.HasBind(SessionFactory))
            {
                CurrentSessionContext.Unbind(SessionFactory);
            }

            if (Session.IsOpen)
            {
                Session.Close();
            }

            Session?.Dispose();
        }
    }
}