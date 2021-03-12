using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public static class RoomAutoFacExtension
    {
        public static void InitRoom(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(RoomService))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(RoomViewModel));
        }
    }
}