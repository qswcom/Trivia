using System;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Module.Game.Interface
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