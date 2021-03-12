using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public static class GameAutoFacExtension
    {
        public static void InitGame(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(GameService))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(GameViewModel));
        }
    }
}