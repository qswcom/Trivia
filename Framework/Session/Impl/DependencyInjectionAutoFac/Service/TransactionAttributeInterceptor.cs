using Castle.DynamicProxy;

namespace Com.Qsw.Framework.Session.Impl
{
    public class TransactionAttributeInterceptor : IInterceptor
    {
        private readonly IAsyncInterceptor asyncInterceptor;

        public TransactionAttributeInterceptor(IAsyncInterceptor asyncInterceptor)
        {
            this.asyncInterceptor = asyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}