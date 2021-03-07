using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Qsw.Framework.MessageQueue.DelegateMessageQueue
{
    internal class GroupConsumerHandler<TValue>
    {
        private readonly object handlerLock;
        private readonly IList<Action<string, TValue>> handlers;
        private readonly Random random;

        public GroupConsumerHandler(string topic, string groupId)
        {
            Topic = topic;
            GroupId = groupId;
            handlers = new List<Action<string, TValue>>();
            handlerLock = new object();
            random = new Random(DateTime.Now.Millisecond);
        }

        public string Topic { get; }
        public string GroupId { get; }

        public void AddHandler(Action<string, TValue> handler)
        {
            lock (handlerLock)
            {
                handlers.Add(handler);
            }
        }

        public Task Handle(string key, TValue value)
        {
            Action<string, TValue> action = null;
            lock (handlerLock)
            {
                var next = random.Next(0, handlers.Count);
                if (next < handlers.Count)
                    action = handlers[next];
            }

            if (action != null)
                return Task.Run(() => action.Invoke(key, value));
            return Task.CompletedTask;
        }
    }
}