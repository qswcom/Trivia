using Autofac;

namespace Com.Qsw.Framework.Session.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitSession(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(ServiceSessionFactoryAsyncInterceptor)).SingleInstance()
                .AsImplementedInterfaces();
            
            containerBuilder.RegisterType(typeof(SessionService)).SingleInstance()
                .AsImplementedInterfaces();
        }
    }
}