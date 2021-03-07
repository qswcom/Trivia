using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Qsw.Framework.MessageQueue.DelegateMessageQueue
{
    internal class DelegateMessageConsumerHandler<TValue>
    {
        private readonly IDictionary<string, GroupConsumerHandler<TValue>> groupConsumerHandlersByGroupId;
        private readonly object handlerLock;

        public DelegateMessageConsumerHandler(string topic)
        {
            Topic = topic;
            groupConsumerHandlersByGroupId = new Dictionary<string, GroupConsumerHandler<TValue>>();
            handlerLock = new object();
        }

        public string Topic { get; }

        public GroupConsumerHandler<TValue> GetGroupConsumerHandler(string groupId)
        {
            lock (handlerLock)
            {
                if (groupConsumerHandlersByGroupId.TryGetValue(groupId,
                    out GroupConsumerHandler<TValue> consumerHandler))
                {
                    return consumerHandler;
                }

                consumerHandler = new GroupConsumerHandler<TValue>(Topic, groupId);
                groupConsumerHandlersByGroupId[groupId] = consumerHandler;
                return consumerHandler;
            }
        }

        public async Task Publish(string key, TValue value)
        {
            IList<GroupConsumerHandler<TValue>> groupConsumerHandlers;
            lock (handlerLock)
            {
                groupConsumerHandlers = groupConsumerHandlersByGroupId.Values.ToList();
            }

            foreach (GroupConsumerHandler<TValue> groupConsumerHandler in groupConsumerHandlers)
            {
                await groupConsumerHandler.Handle(key, value);
            }
        }
    }
}