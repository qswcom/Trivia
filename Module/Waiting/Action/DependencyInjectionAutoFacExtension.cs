using Autofac;

namespace Com.Qsw.Module.Waiting.Action
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitWaitingAction(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(WaitingActionService))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(WaitingNotificationService))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(WaitingListWatchRoomInfoChangedMonitor))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}