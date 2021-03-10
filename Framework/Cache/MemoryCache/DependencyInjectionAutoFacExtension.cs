using Autofac;

namespace Com.Qsw.Framework.Cache.MemoryCache
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitMemoryCache(this Autofac.ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(MemoryCacheService)).SingleInstance().AsImplementedInterfaces();
        }
    }
}