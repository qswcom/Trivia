using Autofac;

namespace Com.Qsw.Framework.Context.Web
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitContextWeb(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(CallContextService)).SingleInstance().AsImplementedInterfaces();
        }
    }
}