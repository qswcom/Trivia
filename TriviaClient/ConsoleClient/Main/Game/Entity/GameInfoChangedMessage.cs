using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class GameInfoChangedMessage
    {
        public GameInfoChangedMessage(GameInfo gameInfo, OperationType operationType)
        {
            GameInfo = gameInfo;
            OperationType = operationType;
        }

        public GameInfo GameInfo { get; }
        public OperationType OperationType { get; }
    }
}