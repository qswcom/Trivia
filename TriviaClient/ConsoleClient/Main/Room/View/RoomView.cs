using System;
using System.ComponentModel;
using Autofac;
using IContainer = Autofac.IContainer;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class RoomView : BaseView
    {
        private readonly IContainer container;
        private readonly IView parent;
        private readonly long roomId;

        private readonly RoomViewModel roomViewModel;

        public RoomView(IContainer container, IView parent, long roomId)
        {
            this.container = container;
            this.parent = parent;
            this.roomId = roomId;

            roomViewModel = container.Resolve<RoomViewModel>();

            CommandInfoByInputDictionary["ri"] = new CommandInfo
            {
                Input = "ri",
                Description = "Try to init room again.",
                Action = Init
            };

            CommandInfoByInputDictionary["rp"] = new CommandInfo
            {
                Input = "rp",
                Description = "Print room.",
                Action = HandleRoomInfo
            };

            CommandInfoByInputDictionary["rl"] = new CommandInfo
            {
                Input = "rl",
                Description = "Left room.",
                Action = roomViewModel.LeftRoomCommand
            };

            CommandInfoByInputDictionary["rs"] = new CommandInfo
            {
                Input = "rs",
                Description = "Start game.",
                Action = roomViewModel.StartGameCommand
            };

            roomViewModel.PropertyChanged += OnPropertyChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            roomViewModel.Dispose();
        }

        public void Init()
        {
            Console.WriteLine("Now in room view.");
            roomViewModel.Bind(roomId).Wait();
            HandleRoomInfo();
        }

        #region Property Changed

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(roomViewModel.Error))
            {
                HandleError();
                return;
            }

            if (e.PropertyName == nameof(roomViewModel.RoomInfo))
            {
                HandleRoomInfo();
            }
        }

        private void HandleError()
        {
            Console.WriteLine(roomViewModel.Error);
        }

        private void HandleRoomInfo()
        {
            Console.WriteLine("Show room details.");
            RoomInfo roomInfo = roomViewModel.RoomInfo;
            if (roomInfo == null)
            {
                Console.WriteLine("Error, can't find room information");
            }
            else
            {
                Console.WriteLine($"Id: {roomInfo.Id}");
                Console.WriteLine($"Organizer: {roomInfo.OrganizerUserId}");
                Console.WriteLine("Users:");
                foreach (RoomUserInfo roomUserInfo in roomInfo.RoomUserInfoByUserIdDictionary.Values)
                {
                    Console.WriteLine($"{roomUserInfo.UserId} belongs to {roomUserInfo.RoomUserRole} and joined {roomUserInfo.JoinDateTime}");
                }
            }
        }

        #endregion
    }
}