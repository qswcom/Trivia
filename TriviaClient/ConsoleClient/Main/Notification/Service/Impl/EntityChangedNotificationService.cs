using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class EntityChangedNotificationService : IEntityChangedNotificationService,
        IEntityChangedNotificationTriggerService
    {
        public event Action<EntityChangedNotificationData> EntityChangedNotification;

        public void Trigger(EntityChangedNotificationData entityChangedNotificationData)
        {
            EntityChangedNotification?.Invoke(entityChangedNotificationData);
        }
    }
}