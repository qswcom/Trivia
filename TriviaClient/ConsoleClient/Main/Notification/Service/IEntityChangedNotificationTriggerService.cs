namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface IEntityChangedNotificationTriggerService
    {
        void Trigger(EntityChangedNotificationData entityChangedNotificationData);
    }
}