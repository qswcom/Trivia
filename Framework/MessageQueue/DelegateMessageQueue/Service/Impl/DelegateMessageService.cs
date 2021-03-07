using System;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;

namespace Com.Qsw.Framework.MessageQueue.DelegateMessageQueue
{
    public class DelegateMessageService : IMessageService
    {
        public async Task Publish<TValue>(string topic, string key, TValue value)
        {
            var consumerHandler = LocalMessageCustomerConsumerFactory.GetConsumerHandler<TValue>(topic);

            await consumerHandler.Publish(key, value);
        }

        public async Task AddConsumer<TValue>(string groupId, string topic, Action<string, TValue> messageHandler)
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                throw new ArgumentNullException(nameof(groupId));
            }

            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new ArgumentNullException(nameof(topic));
            }
            
            var consumerHandler = LocalMessageCustomerConsumerFactory.GetConsumerHandler<TValue>(topic);
            var groupConsumerHandler = consumerHandler.GetGroupConsumerHandler(groupId);
            groupConsumerHandler.AddHandler(messageHandler);

            await Task.CompletedTask;
        }
    }
}