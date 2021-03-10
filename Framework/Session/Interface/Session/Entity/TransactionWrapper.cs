using System;
using System.Threading.Tasks;
using NHibernate;

namespace Com.Qsw.Framework.Session.Interface
{
    public class TransactionWrapper : IDisposable
    {
        public TransactionWrapper(SessionWrapper sessionWrapper, ITransaction transaction, bool isCreator)
        {
            SessionWrapper = sessionWrapper;
            Transaction = transaction;
            IsCreator = isCreator;
        }

        public SessionWrapper SessionWrapper { get; }
        public ITransaction Transaction { get; }
        public bool IsCreator { get; }

        public async Task Commit()
        {
            if (!IsCreator)
            {
                return;
            }

            if (SessionWrapper.Session.IsConnected && Transaction.IsActive)
            {
                await Transaction.CommitAsync();
            }
            else
            {
                throw new ApplicationException("Session closed.");
            }
        }

        public async Task Rollback()
        {
            if (!IsCreator)
            {
                return;
            }

            if (SessionWrapper.Session.IsConnected && Transaction.IsActive)
            {
                await Transaction.RollbackAsync();
            }
            else
            {
                throw new ApplicationException("Session closed.");
            }
        }

        public void Dispose()
        {
            if (!IsCreator)
            {
                return;
            }

            try
            {
                if (SessionWrapper.Session.IsConnected && Transaction.IsActive)
                {
                    if (!Transaction.WasCommitted)
                    {
                        Transaction.Rollback();
                    }
                }
            }
            catch (Exception)
            {
                // Ignore
            }
            

            Transaction?.Dispose();
        }
    }
}