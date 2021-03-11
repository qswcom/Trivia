using System;

namespace Com.Qsw.Module.Game.Interface
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