using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface IEntityChangedNotificationService
    {
        event Action<EntityChangedNotificationData> EntityChangedNotification;
    }
}