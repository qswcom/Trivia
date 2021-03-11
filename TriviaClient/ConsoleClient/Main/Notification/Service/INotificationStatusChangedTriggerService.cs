namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface INotificationStatusChangedTriggerService
    {
        void TriggerClosed();
        void TriggerReconnected();
    }
}