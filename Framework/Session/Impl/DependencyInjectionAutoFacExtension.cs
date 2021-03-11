using Autofac;

namespace Com.Qsw.Framework.Session.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitSession(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(LockAsyncInterceptor)).SingleInstance()
                .AsImplementedInterfaces();
            
            containerBuilder.RegisterType(typeof(TransactionAsyncInterceptor)).SingleInstance()
                .AsImplementedInterfaces();
            
            containerBuilder.RegisterType(typeof(SessionService)).SingleInstance()
                .AsImplementedInterfaces();
        }
    }
}