using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public enum GameUserQuestionState
    {
        NotStart,
        Process,
        Success,
        Failed,
        Overtime,
        UnApply
    }
}