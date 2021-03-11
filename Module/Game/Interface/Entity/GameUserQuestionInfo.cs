using System;

namespace Com.Qsw.Module.Game.Interface
{
    [Serializable]
    public class GameUserQuestionInfo
    {
        public DateTime ExpireDateTime { get; set; }
        public DateTime AnsweredDateTime { get; set; }
        public GameUserQuestionState GameUserQuestionState { get; set; }
    }
}