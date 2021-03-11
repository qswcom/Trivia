using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
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