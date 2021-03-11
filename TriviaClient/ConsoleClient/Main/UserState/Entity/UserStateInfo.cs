using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class UserStateInfo
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public UserState UserState { get; set; }
        public long RoomId { get; set; }
        public long GameId { get; set; }
    }
}