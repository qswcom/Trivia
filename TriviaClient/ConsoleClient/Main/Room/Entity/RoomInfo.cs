using System;
using System.Collections.Generic;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class RoomInfo 
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