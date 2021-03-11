using System;
using System.Threading.Tasks;
using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Trivia game.");
            Console.WriteLine("Please enter you user name:(any string)");
            string userName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("User name invalid.");
                return;
            }

            var userInfo = new UserInfo
            {
                UserId = userName
            };

            IContainer container = InitDependency();
            var notificationService = container.Resolve<INotificationService>();
            try
            {
                await notificationService.ConnectToServer();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on connect to remote server, error message: {e.Message}");
            }
        }

        #region Dependency

        private static IContainer InitDependency()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType(typeof(ServerInfoService)).SingleInstance().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(NotificationService)).SingleInstance().AsImplementedInterfaces();

            return containerBuilder.Build();
        }

        #endregion
    }
}