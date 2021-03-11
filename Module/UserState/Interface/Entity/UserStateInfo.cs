using System;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Module.UserState.Interface
{
    [Serializable]
    public class UserStateInfo : IEntityId
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public UserState UserState { get; set; }
        public long RoomId { get; set; }
        public long GameId { get; set; }
    }
}