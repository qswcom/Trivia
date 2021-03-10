using System;
using System.Threading.Tasks;
using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Trivia game.");
            Console.WriteLine("Please enter you user name:(any string)");
            string userName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("User name invalid.");
                return;
            }

            IContainer container = InitDependency();
            var notificationService = container.Resolve<INotificationService>();
            try
            {
                await notificationService.ConnectToServer(userName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on connect to remote server, error message: {e.Message}");
                return;
            }
            
            
        }

        #region Dependency
        
        private static IContainer InitDependency()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType(typeof(ServerInfoService)).SingleInstance().AsImplementedInterfaces();
            containerBuilder.RegisterType(typeof(NotificationService)).SingleInstance().AsImplementedInterfaces();

            return containerBuilder.Build();
        }
        
        #endregion
    }
}