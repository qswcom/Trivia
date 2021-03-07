using System;
using System.Threading.Tasks;

namespace Com.Qsw.Framework.MessageQueue.Interface
{
    public interface IMessageService
    {
        Task Publish<TValue>(string topic, string key, TValue value);
        Task AddConsumer<TValue>(string groupId, string topic, Action<string, TValue> messageHandler);
    }
}