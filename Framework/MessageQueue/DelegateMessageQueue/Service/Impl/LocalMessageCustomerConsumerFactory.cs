using System;
using System.Collections.Generic;

namespace Com.Qsw.Framework.MessageQueue.DelegateMessageQueue
{
    internal static class LocalMessageCustomerConsumerFactory
    {
        private static readonly IDictionary<string, object> consumerHandlersByTopic;
        private static readonly object handlersLock;

        static LocalMessageCustomerConsumerFactory()
        {
            consumerHandlersByTopic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            handlersLock = new object();
        }

        public static DelegateMessageConsumerHandler<TValue> GetConsumerHandler<TValue>(string topic)
        {
            lock (handlersLock)
            {
                if (consumerHandlersByTopic.TryGetValue(topic, out var consumerHandler))
                {
                    if (consumerHandler is DelegateMessageConsumerHandler<TValue> result)
                    {
                        return result;
                    }

                    throw new ApplicationException($"Message topic {topic} has different key value type.");
                }

                consumerHandler = new DelegateMessageConsumerHandler<TValue>(topic);
                consumerHandlersByTopic[topic] = consumerHandler;
                return consumerHandler as DelegateMessageConsumerHandler<TValue>;
            }
        }
    }
}