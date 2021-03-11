using System;

namespace Com.Qsw.Module.UserState.Interface
{
    [Serializable]
    public class UserConnectionStatus
    {
        public string UserId { get; set; }
        public bool IsConnected { get; set; }
    }
}