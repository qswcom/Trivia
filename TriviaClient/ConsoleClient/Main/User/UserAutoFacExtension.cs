using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public static class UserAutoFacExtension
    {
        public static void InitUser(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(UserInfoService))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}