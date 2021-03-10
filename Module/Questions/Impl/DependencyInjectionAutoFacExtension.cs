using Autofac;
using Autofac.Extras.DynamicProxy;
using Com.Qsw.Framework.Session.Impl;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Question.Impl
{
    public static class DependencyInjectionAutoFacExtension
    {
        public static void InitQuestion(this ContainerBuilder containerBuilder)
        {
            // Question Info Statistic
            containerBuilder.RegisterType(typeof(QuestionInfoStatisticRepository)).SingleInstance()
                .AsImplementedInterfaces();
            containerBuilder
                .RegisterDecorator<QuestionInfoStatisticServicePermissionDecorator, IQuestionInfoStatisticService>();
            containerBuilder.RegisterType(typeof(QuestionInfoStatisticService))
                .SingleInstance().AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionAttributeInterceptor));

            // Question
            containerBuilder.RegisterType(typeof(QuestionInfoRepository))
                .SingleInstance().AsImplementedInterfaces();
            containerBuilder.RegisterDecorator<QuestionInfoServicePermissionDecorator, IQuestionInfoService>();
            containerBuilder.RegisterType(typeof(QuestionInfoService))
                .SingleInstance().AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionAttributeInterceptor));
        }
    }
}