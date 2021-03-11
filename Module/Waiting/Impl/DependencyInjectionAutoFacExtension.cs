using Autofac;

namespace Com.Qsw.Module.Waiting.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitWaiting(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(WaitingService))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}