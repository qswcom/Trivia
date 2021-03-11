using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Com.Qsw.Framework.Cache.MemoryCache;
using Com.Qsw.Framework.Context.Web;
using Com.Qsw.Framework.MessageQueue.DelegateMessageQueue;
using Com.Qsw.Framework.Session.Impl;
using Com.Qsw.Module.Notification.Impl;
using Com.Qsw.Module.Question.Impl;
using Com.Qsw.Module.UserState.Impl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NHibernate;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Com.Qsw.TriviaServer.AppServer.Main
{
    public class MainService : IMainService
    {
        private readonly ILogger logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly IConfiguration configuration;
        private ISessionFactory sessionFactory;

        public MainService(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory.CreateLogger<MainService>();
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
        }

        public IServiceProvider Start()
        {
            try
            {
                logger.LogInformation("Start to init session factory.");
                sessionFactory = InitSessionFactory();
                logger.LogInformation("Start to Init dependency injection.");

                logger.LogInformation("Start to start web.");
                IServiceProvider serviceProvider = StartWeb();
                logger.LogInformation("Success to start web.");
                return serviceProvider;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on start main server.");
                throw;
            }
        }

        #region Web

        private IServiceProvider StartWeb()
        {
            string url = configuration["Url"];
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(
                    configurationBuilder => { configurationBuilder.AddConfiguration(configuration); })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(InitDependencyInjection))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(url)
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<Startup>();
                });

            IHost host = hostBuilder.Build();
            IServiceProvider serviceProvider = host.Services;
            foreach (IMonitor service in serviceProvider.GetServices<IMonitor>())
            {
                service.Start();
            }

            host.Start();
            return serviceProvider;
        }

        #endregion

        #region AutoFac

        private void InitDependencyInjection(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterInstance(loggerFactory).AsImplementedInterfaces();
            containerBuilder.RegisterInstance(sessionFactory).AsImplementedInterfaces();

            containerBuilder.InitMemoryCache();
            containerBuilder.InitDelegateMessageQueue();
            containerBuilder.InitSession();
            containerBuilder.InitContextWeb();

            containerBuilder.InitNotification();
            containerBuilder.InitQuestion();
            containerBuilder.InitUserState();
        }

        #endregion

        #region Session

        private ISessionFactory InitSessionFactory()
        {
            var hibernateService = new HibernateService(configuration);
            hibernateService.InitHibernate();
            return hibernateService.GetSessionFactory();
        }

        #endregion
    }
}