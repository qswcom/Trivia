using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public static class UserStateAutoFacExtension
    {
        public static void InitUserState(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(UserStateInfoService))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(UserStateInfoViewModel));
        }
    }
}