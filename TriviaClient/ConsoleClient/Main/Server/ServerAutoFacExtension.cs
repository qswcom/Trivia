using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public static class ServerAutoFacExtension
    {
        public static void InitServer(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(ServerInfoService))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}