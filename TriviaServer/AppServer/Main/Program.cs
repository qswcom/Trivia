using System;
using System.IO;
using System.Linq;
using System.Threading;
using apache.log4net.Extensions.Logging;
using Com.Qsw.Module.Notification.Impl;
using Com.Qsw.Module.Question.Impl;
using Com.Qsw.TriviaServer.AppServer.Main.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Com.Qsw.TriviaServer.AppServer.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string rootDictionary = AppContext.BaseDirectory;
            Directory.SetCurrentDirectory(rootDictionary);
            ILoggerFactory loggerFactory = InitLogging();
            IConfigurationRoot configurationRoot = InitConfiguration();
            IMainService mainService = new MainService(loggerFactory, configurationRoot);
            IServiceProvider serviceProvider = mainService.Start();

            if (args.Contains("--test", StringComparer.InvariantCultureIgnoreCase) ||
                args.Contains("-t", StringComparer.InvariantCultureIgnoreCase))
            {
                {
                    var databaseInitService = new DatabaseInitService(configurationRoot);
                    try
                    {
                        databaseInitService.InitDatabase().Wait();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error happens when init database, error message: {e.Message}");
                    }
                }

                {
                    var questionInfoStatisticRepository = serviceProvider.GetService<IQuestionInfoStatisticRepository>();
                    var questionInfoRepository = serviceProvider.GetService<IQuestionInfoRepository>();
                    var questionInitService = new QuestionInitService(questionInfoRepository, questionInfoStatisticRepository);
                    try
                    {
                        questionInitService.Init().Wait();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error happens when init question, error message: {e.Message}");
                    }
                }
                
            }

            while (true)
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        #region Init

        private static ILoggerFactory InitLogging()
        {
            ILoggerFactory loggerFactory = new LoggerFactory().AddLog4Net(new Log4NetSettings
            {
                ConfigFile = Path.Combine(Environment.CurrentDirectory, "configs/log4net.config")
            });
            return loggerFactory;
        }

        private static IConfigurationRoot InitConfiguration()
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("configs/appsettings.json", true, true)
                .AddJsonFile($"configs/appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        #endregion
    }
}