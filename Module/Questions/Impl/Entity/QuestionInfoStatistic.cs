using System;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Question.Impl
{
    [Serializable]
    public class QuestionInfoStatistic : EntityBase
    {
        public QuestionCategory QuestionCategory { get; set; }
        public int Difficult { get; set; }
        public int QuestionCount { get; set; }
    }
}