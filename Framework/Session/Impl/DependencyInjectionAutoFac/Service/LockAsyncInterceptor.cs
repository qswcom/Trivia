using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Framework.Session.Impl
{
    public class LockAsyncInterceptor : AsyncInterceptorBase, ILockAsyncInterceptor
    {
        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            LockAttribute lockAttribute = LockUtils.FindLockAttribute(methodInfo);
            bool lockRequired = lockAttribute != null;

            if (lockRequired)
            {
                lock (invocation.InvocationTarget)
                {
                    proceed(invocation, proceedInfo).Wait();
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
            LockAttribute lockAttribute = LockUtils.FindLockAttribute(methodInfo);
            bool lockRequired = lockAttribute != null;

            if (lockRequired)
            {
                lock (invocation.InvocationTarget)
                {
                    TResult retObject = proceed(invocation, proceedInfo).Result;
                    return retObject;
                }
            }

            {
                TResult retObject = await proceed(invocation, proceedInfo).ConfigureAwait(false);
                return retObject;
            }
        }
    }
}