using Autofac;
using Autofac.Extras.DynamicProxy;
using Com.Qsw.Framework.Session.Impl;

namespace Com.Qsw.Module.Game.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitGame(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(GameInfoRepository))
                .SingleInstance().AsImplementedInterfaces();
            
            containerBuilder.RegisterType(typeof(GameInfoService))
                .SingleInstance().AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(LockInterceptor));
            containerBuilder.RegisterType(typeof(GameQuestionService))
                .SingleInstance().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(GameStateService))
                .SingleInstance().AsImplementedInterfaces();
            
            containerBuilder.RegisterType(typeof(GameInfoWatchTimerExpiredMonitor))
                .SingleInstance().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(NotificationWatchGameInfoChangedMonitor))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}