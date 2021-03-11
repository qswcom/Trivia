using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface INotificationStatusChangedService
    {
        event Action CloseEvent;
        event Action ReconnectedEvent;
    }
}