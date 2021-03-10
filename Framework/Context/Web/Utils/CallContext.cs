using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Com.Qsw.Framework.Context.Web
{
    public static class CallContext
    {
        private static readonly ConcurrentDictionary<string, AsyncLocal<object>> state =
            new ConcurrentDictionary<string, AsyncLocal<object>>();

        public static void SetData(string name, object data)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;
        }

        public static object GetData(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
        }
    }
}