using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly INotificationStatusChangedService notificationStatusChangedService;
        private bool isStopRequest;

        public MainViewModel(INotificationStatusChangedService notificationStatusChangedService)
        {
            this.notificationStatusChangedService = notificationStatusChangedService;
            this.notificationStatusChangedService.CloseEvent += OnConnectionClosed;
            this.notificationStatusChangedService.ReconnectedEvent += OnReconnected;
        }

        public bool IsStopRequest
        {
            get => isStopRequest;
            set => SetValue(ref isStopRequest, value);
        }

        public void Dispose()
        {
            notificationStatusChangedService.CloseEvent -= OnConnectionClosed;
            notificationStatusChangedService.ReconnectedEvent -= OnReconnected;
        }
        
        #region Command
        
        public void QuitCommand()
        {
            isStopRequest = true;
        }
        
        #endregion

        #region Connection Management

        private void OnConnectionClosed()
        {
            //TODO: Realize later.
        }

        private void OnReconnected()
        {
            //TODO: Realize later.
        }

        #endregion

       
    }
}