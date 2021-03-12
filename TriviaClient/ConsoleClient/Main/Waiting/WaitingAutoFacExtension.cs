using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public static class WaitingAutoFacExtension
    {
        public static void InitWaiting(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(WaitingService))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(WaitingViewModel));
        }
    }
}