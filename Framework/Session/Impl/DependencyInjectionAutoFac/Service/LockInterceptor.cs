using Castle.DynamicProxy;

namespace Com.Qsw.Framework.Session.Impl
{
    public class LockInterceptor : IInterceptor
    {
        private readonly IAsyncInterceptor asyncInterceptor;

        public LockInterceptor(ILockAsyncInterceptor asyncInterceptor)
        {
            this.asyncInterceptor = asyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}