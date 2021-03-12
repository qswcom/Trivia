using System;
using System.Collections.Generic;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class GameQuestionInfo
    {
        public GameQuestionInfo()
        {
            QuestionInfoList = new List<QuestionInfo>();
        }

        public IList<QuestionInfo> QuestionInfoList { get; set; }
        public int CurrentCount => QuestionInfoList.Count;
    }
}