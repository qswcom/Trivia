using System;
using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class MainView : BaseView
    {
        private readonly IContainer container;

        private readonly MainViewModel mainViewModel;

        public MainView(IContainer container)
        {
            this.container = container;
            mainViewModel = container.Resolve<MainViewModel>();

            CommandInfoByInputDictionary["h"] = new CommandInfo
            {
                Input = "h",
                Description = "Show help information.",
                Action = PrintCommands
            };
            CommandInfoByInputDictionary["c"] = new CommandInfo
            {
                Input = "c",
                Description = "Clear console.",
                Action = Clear
            };
            CommandInfoByInputDictionary["q"] = new CommandInfo
            {
                Input = "q",
                Description = "Quit.",
                Action = mainViewModel.QuitCommand
            };
           
        }

        public override void Dispose()
        {
            base.Dispose();
            mainViewModel.Dispose();
        }

        public void Init()
        {
            var userStateInfoView = new UserStateInfoView(container, this);
            ChildView = userStateInfoView;
            userStateInfoView.Init();
        }

        public void Run()
        {
            while (!mainViewModel.IsStopRequest)
            {
                string input = Console.ReadLine();
                bool isHandled = HandleInput(input);
                if (!isHandled)
                {
                    Console.WriteLine($"Can't recognize input {input}.");
                }
            }
        }
        
        #region Command
        
        private void Clear()
        {
            Console.Clear();
        }
        
        #endregion
    }
}