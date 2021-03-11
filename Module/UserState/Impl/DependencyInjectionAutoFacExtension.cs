using Autofac;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitUserState(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(UserStateInfoRepository))
                .SingleInstance().AsImplementedInterfaces();
            
            containerBuilder.RegisterDecorator<UserStateInfoServicePermissionDecorator, IUserStateInfoService>();
            containerBuilder.RegisterDecorator<UserStateInfoServiceValidationDecorator, IUserStateInfoService>();
            containerBuilder.RegisterDecorator<UserStateInfoServiceMessageDecorator, IUserStateInfoService>();
            containerBuilder.RegisterType(typeof(UserStateInfoService))
                .SingleInstance().AsImplementedInterfaces();
            
            containerBuilder.RegisterType(typeof(NotificationWatchUserStateInfoChangedMonitor))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}