using Autofac;
using Autofac.Extras.DynamicProxy;
using Com.Qsw.Framework.Session.Impl;

namespace Com.Qsw.Module.Question.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitQuestion(this ContainerBuilder containerBuilder)
        {
            // Question Info Statistic
            containerBuilder.RegisterType(typeof(QuestionInfoStatisticRepository))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(QuestionInfoStatisticService))
                .SingleInstance().AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionInterceptor));

            // Question
            containerBuilder.RegisterType(typeof(QuestionInfoRepository))
                .SingleInstance().AsImplementedInterfaces();

            containerBuilder.RegisterType(typeof(QuestionInfoService))
                .SingleInstance().AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionInterceptor));
        }
    }
}