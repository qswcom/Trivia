using System;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    [Serializable]
    public class UserStateInfoChangedMessage
    {
        public UserStateInfoChangedMessage(UserStateInfo userStateInfo)
        {
            UserStateInfo = userStateInfo;
        }

        public UserStateInfo UserStateInfo { get; }
    }
}