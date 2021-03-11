using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
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