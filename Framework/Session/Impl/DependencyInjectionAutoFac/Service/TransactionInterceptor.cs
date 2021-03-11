using Castle.DynamicProxy;

namespace Com.Qsw.Framework.Session.Impl
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly IAsyncInterceptor asyncInterceptor;

        public TransactionInterceptor(ITransactionAsyncInterceptor asyncInterceptor)
        {
            this.asyncInterceptor = asyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}