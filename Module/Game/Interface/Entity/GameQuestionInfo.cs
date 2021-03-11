using System;
using System.Collections.Generic;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Game.Interface
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