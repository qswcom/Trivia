using System;
using System.ComponentModel;
using Autofac;
using IContainer = Autofac.IContainer;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class UserStateInfoView : BaseView
    {
        private readonly IContainer container;
        private readonly IView parent;
        private readonly UserStateInfoViewModel userStateInfoViewModel;

        public UserStateInfoView(IContainer container, IView parent)
        {
            this.container = container;
            this.parent = parent;

            userStateInfoViewModel = container.Resolve<UserStateInfoViewModel>();

            CommandInfoByInputDictionary["ui"] = new CommandInfo
            {
                Input = "ui",
                Description = "Try to init user state info again.",
                Action = Init
            };

            userStateInfoViewModel.PropertyChanged += OnPropertyChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            userStateInfoViewModel.Dispose();
        }

        public void Init()
        {
            Console.WriteLine("Now in user state info view.");
            userStateInfoViewModel.Bind().Wait();
            HandleUserStateChanged();
        }

        #region Property Changed

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(userStateInfoViewModel.Error))
            {
                HandleError();
                return;
            }

            if (e.PropertyName == nameof(userStateInfoViewModel.UserState))
            {
                HandleUserStateChanged();
            }
        }

        private void HandleError()
        {
            Console.WriteLine(userStateInfoViewModel.Error);
        }

        private void HandleUserStateChanged()
        {
            switch (userStateInfoViewModel.UserState)
            {
                case UserState.Waiting:
                    if (ChildView is WaitingView)
                    {
                    }
                    else
                    {
                        IView oldChildView = ChildView;
                        var waitingView = new WaitingView(container, parent);
                        waitingView.Init();
                        ChildView = waitingView;
                        oldChildView?.Dispose();
                    }

                    break;
                case UserState.Room:
                    if (ChildView is RoomView)
                    {
                    }
                    else
                    {
                        IView oldChildView = ChildView;
                        var roomView = new RoomView(container, parent, userStateInfoViewModel.RoomId);
                        roomView.Init();
                        ChildView = roomView;
                        oldChildView?.Dispose();
                    }

                    break;
                case UserState.Game:
                    if (ChildView is GameView)
                    {
                    }
                    else
                    {
                        IView oldChildView = ChildView;
                        var gameView = new GameView(container, parent, userStateInfoViewModel.GameId);
                        gameView.Init();
                        ChildView = gameView;
                        oldChildView?.Dispose();
                    }

                    break;
            }
        }

        #endregion
    }
}