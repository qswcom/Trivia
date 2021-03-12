using System;
using System.ComponentModel;
using Autofac;
using IContainer = Autofac.IContainer;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class WaitingView : BaseView
    {
        private readonly IContainer container;
        private readonly IView parent;

        private readonly WaitingViewModel waitingViewModel;

        public WaitingView(IContainer container, IView parent)
        {
            this.container = container;
            this.parent = parent;

            waitingViewModel = container.Resolve<WaitingViewModel>();

            CommandInfoByInputDictionary["wi"] = new CommandInfo
            {
                Input = "wi",
                Description = "Try to init waiting again.",
                Action = Init
            };

            CommandInfoByInputDictionary["wp"] = new CommandInfo
            {
                Input = "wp",
                Description = "Print rooms.",
                Action = HandleRoomInfoDic
            };

            CommandInfoByInputDictionary["wc"] = new CommandInfo
            {
                Input = "wc",
                Description = "Create a new room.",
                Action = waitingViewModel.CreateRoomCommand
            };

            CommandInfoByInputDictionary["wj"] = new CommandInfo
            {
                Input = "wj",
                Description = "Join room.",
                Action = waitingViewModel.JoinRoomCommand
            };

            waitingViewModel.PropertyChanged += OnPropertyChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            waitingViewModel.Dispose();
        }

        public void Init()
        {
            Console.WriteLine("Now in waiting view.");
            waitingViewModel.Bind().Wait();
            HandleRoomInfoDic();
        }

        #region Property Changed

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(waitingViewModel.Error))
            {
                HandleError();
                return;
            }

            if (e.PropertyName == nameof(waitingViewModel.RoomInfoDic))
            {
                HandleRoomInfoDic();
            }
        }

        private void HandleError()
        {
            Console.WriteLine(waitingViewModel.Error);
        }

        private void HandleRoomInfoDic()
        {
            Console.WriteLine("Show all waiting rooms.");
            Console.WriteLine("Id      \t\tUser Count\t\tOrganizer");
            foreach (RoomInfo roomInfo in waitingViewModel.RoomInfoDic.Values)
            {
                Console.WriteLine(
                    $"{roomInfo.Id:D8}\t\t{roomInfo.RoomUserInfoByUserIdDictionary.Count:D9}\t\t{roomInfo.OrganizerUserId}");
            }
        }

        #endregion
    }
}