using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class NotificationStatusChangedTriggerService : INotificationStatusChangedService,
        INotificationStatusChangedTriggerService
    {
        public event Action CloseEvent;
        public event Action ReconnectedEvent;

        public void TriggerClosed()
        {
            CloseEvent?.Invoke();
        }

        public void TriggerReconnected()
        {
            ReconnectedEvent?.Invoke();
        }
    }
}