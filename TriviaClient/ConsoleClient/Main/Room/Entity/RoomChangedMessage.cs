using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class RoomChangedMessage
    {
        public RoomChangedMessage(RoomInfo roomInfo, OperationType operationType)
        {
            RoomInfo = roomInfo;
            OperationType = operationType;
        }

        public RoomInfo RoomInfo { get; }
        public OperationType OperationType { get; }
    }
}