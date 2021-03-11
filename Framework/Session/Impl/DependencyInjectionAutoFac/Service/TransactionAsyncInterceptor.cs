using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Framework.Session.Impl
{
    public class TransactionAsyncInterceptor : AsyncInterceptorBase, ITransactionAsyncInterceptor
    {
        private readonly ISessionService sessionService;

        public TransactionAsyncInterceptor(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            TransactionAttribute transactionAttribute = TransactionUtils.FindTransactionAttribute(methodInfo);
            bool bindingRequired = transactionAttribute != null;

            if (bindingRequired)
            {
                using (SessionWrapper sessionWrapper = sessionService.GetCurrentSession(transactionAttribute.ReadOnly))
                {
                    using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction(
                        transactionAttribute.ReadOnly, transactionAttribute.IsolationLevel))
                    {
                        await proceed(invocation, proceedInfo).ConfigureAwait(false);
                        await transactionWrapper.Commit().ConfigureAwait(false);
                    }
                }
            }
            else
            {
                await proceed(invocation, proceedInfo).ConfigureAwait(false);
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation,
            IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            TransactionAttribute transactionAttribute = TransactionUtils.FindTransactionAttribute(methodInfo);
            bool bindingRequired = transactionAttribute != null;

            if (bindingRequired)
            {
                using (SessionWrapper sessionWrapper = sessionService.GetCurrentSession(transactionAttribute.ReadOnly))
                {
                    using (TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction(
                        transactionAttribute.ReadOnly, transactionAttribute.IsolationLevel))
                    {
                        TResult retObject = await proceed(invocation, proceedInfo).ConfigureAwait(false);
                        await transactionWrapper.Commit().ConfigureAwait(false);
                        return retObject;
                    }
                }
            }

            {
                TResult retObject = await proceed(invocation, proceedInfo).ConfigureAwait(false);
                return retObject;
            }
        }
    }
}