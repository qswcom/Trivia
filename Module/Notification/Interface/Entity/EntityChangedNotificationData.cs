using System;

namespace Com.Qsw.Module.Notification.Interface
{
    [Serializable]
    public class EntityChangedNotificationData
    {
        public EntityChangedNotificationData(string entityType, string entityChangedMessageJson)
        {
            EntityType = entityType;
            EntityChangedMessageJson = entityChangedMessageJson;
        }

        public string EntityType { get; }
        public string EntityChangedMessageJson { get; }
    }
}