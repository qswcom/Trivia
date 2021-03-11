using Autofac;
using Autofac.Extras.DynamicProxy;
using Com.Qsw.Framework.Session.Impl;

namespace Com.Qsw.Module.Room.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitRoom(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(RoomInfoRepository))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(RoomInfoService))
                .SingleInstance().AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(LockInterceptor));
            
            containerBuilder.RegisterType(typeof(NotificationWatchRoomInfoChangedMonitor))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}