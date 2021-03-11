using Autofac;

namespace Com.Qsw.Module.Game.Action
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitGameAction(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(GameActionService))
                .SingleInstance().AsImplementedInterfaces();
        }
    }
}