using Autofac;

namespace Com.Qsw.Module.Game.Timer
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitGameTimer(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(GameTimerService))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}