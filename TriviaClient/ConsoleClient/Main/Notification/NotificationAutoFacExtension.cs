using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public static class NotificationAutoFacExtension
    {
        public static void InitNotification(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(EntityChangedNotificationService))
                .SingleInstance().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(NotificationService))
                .SingleInstance().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(NotificationStatusChangedTriggerService))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}