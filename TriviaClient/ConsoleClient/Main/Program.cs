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
            Console.WriteLine("In any time, you can input 'h' for help.");
            Console.WriteLine("Please enter you user name:(any string)");
            string userName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("User name invalid, abort.");
                return;
            }

            var userInfo = new UserInfo
            {
                UserId = userName
            };

            IContainer container = InitDependency();
            var userInfoService = container.Resolve<IUserInfoService>();
            userInfoService.UserInfo = userInfo;
            var notificationService = container.Resolve<INotificationService>();
            try
            {
                await notificationService.ConnectToServer();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on connect to remote server, error message: {e.Message}");
                return;
            }

            var mainView = new MainView(container);
            mainView.Init();
            mainView.Run();
            mainView.Dispose();
            Console.WriteLine("Thank you for playing Trivia game, Good bye.");
        }

        #region Dependency

        private static IContainer InitDependency()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.InitNotification();
            containerBuilder.InitServer();
            containerBuilder.InitUser();
            
            containerBuilder.InitUserState();
            containerBuilder.InitWaiting();
            containerBuilder.InitRoom();
            containerBuilder.InitGame();

            containerBuilder.RegisterType(typeof(MainViewModel));
            return containerBuilder.Build();
        }

        #endregion
    }
}