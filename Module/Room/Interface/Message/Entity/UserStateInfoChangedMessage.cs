using System;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Module.Room.Interface
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