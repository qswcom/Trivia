using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface IView : IDisposable
    {
        IView ChildView { get; }
        void PrintCommands();
        bool HandleInput(string input);
    }
}