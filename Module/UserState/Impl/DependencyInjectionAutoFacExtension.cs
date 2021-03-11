using Autofac;
using Autofac.Extras.DynamicProxy;
using Com.Qsw.Framework.Session.Impl;

namespace Com.Qsw.Module.UserState.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitUserState(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(UserStateInfoRepository))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(UserStateInfoService))
                .SingleInstance().AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(LockInterceptor));

            containerBuilder.RegisterType(typeof(NotificationWatchUserStateInfoChangedMonitor))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(UserConnectionStatusService))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}