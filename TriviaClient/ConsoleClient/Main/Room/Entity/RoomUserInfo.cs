using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class RoomUserInfo
    {
        public string UserId { get; set; }
        public RoomUserRole RoomUserRole { get; set; }
        public DateTime JoinDateTime { get; set; }
    }
}