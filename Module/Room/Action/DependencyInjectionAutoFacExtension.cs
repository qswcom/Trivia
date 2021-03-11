using Autofac;

namespace Com.Qsw.Module.Room.Action
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitRoomAction(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(RoomActionService))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}