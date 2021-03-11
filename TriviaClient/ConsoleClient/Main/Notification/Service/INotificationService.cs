using System;
using System.Threading.Tasks;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface INotificationService : IDisposable
    {
        Task ConnectToServer();
    }
}