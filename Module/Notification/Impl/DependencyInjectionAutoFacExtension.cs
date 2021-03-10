using Autofac;

namespace Com.Qsw.Module.Notification.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitNotification(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(ConnectionService)).SingleInstance().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(NotificationService)).SingleInstance().AsImplementedInterfaces();
        }
    }
}