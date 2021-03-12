using System;
using System.Collections.Generic;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public abstract class BaseView : IView
    {
        protected BaseView()
        {
            CommandInfoByInputDictionary =
                new Dictionary<string, CommandInfo>(StringComparer.InvariantCultureIgnoreCase);
        }
        
        protected IDictionary<string, CommandInfo> CommandInfoByInputDictionary { get; }


        public IView ChildView { get; protected set; }

        public virtual void Dispose()
        {
            ChildView?.Dispose();
        }

        #region Input

        public virtual void PrintCommands()
        {
            foreach (CommandInfo commandInfo in CommandInfoByInputDictionary.Values)
            {
                Console.WriteLine($"{commandInfo.Input}\t{commandInfo.Description}");
            }

            ChildView?.PrintCommands();
        }

        public virtual bool HandleInput(string input)
        {
            if (ChildView != null)
            {
                bool isHandled = ChildView.HandleInput(input);
                if (isHandled)
                {
                    return true;
                }
            }

            return SelfHandleInput(input);
        }

        protected virtual bool SelfHandleInput(string input)
        {
            if (CommandInfoByInputDictionary.TryGetValue(input, out CommandInfo commandInfo))
            {
                commandInfo.Action.Invoke();
                return true;
            }

            return false;
        }

        #endregion
    }
}