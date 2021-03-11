using System;
using System.Collections.Generic;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Module.Room.Interface
{
    [Serializable]
    public class RoomInfo : IEntityId
    {
        public RoomInfo()
        {
            RoomUserInfoByUserIdDictionary = new Dictionary<string, RoomUserInfo>();
        }

        public long Id { get; set; }
        public IDictionary<string, RoomUserInfo> RoomUserInfoByUserIdDictionary { get; set; }
        public string OrganizerUserId { get; set; }
    }
}