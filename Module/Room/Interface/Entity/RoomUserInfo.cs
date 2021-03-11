using System;

namespace Com.Qsw.Module.Room.Interface
{
    [Serializable]
    public class RoomUserInfo
    {
        public string UserId { get; set; }
        public RoomUserRole RoomUserRole { get; set; }
        public DateTime JoinDateTime { get; set; }
    }
}