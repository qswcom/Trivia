using System;
using System.Collections.Generic;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class GameUserInfo
    {
        public GameUserInfo()
        {
            GameUserQuestionInfoList = new List<GameUserQuestionInfo>();
        }

        public string UserId { get; set; }
        public IList<GameUserQuestionInfo> GameUserQuestionInfoList { get; set; }
        public GameUserState GameUserState { get; set; }
    }
}