using Autofac;

namespace Com.Qsw.Framework.MessageQueue.DelegateMessageQueue
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitDelegateMessageQueue(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(DelegateMessageService)).SingleInstance().AsImplementedInterfaces();
        }
    }
}