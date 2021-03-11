using Com.Qsw.Module.Game.Interface;

namespace Com.Qsw.Module.Game.Impl
{
    public interface IGameStateService
    {
        GameInfo AnalyzeByGameUserQuestionStateChanged(GameInfo gameInfo, GameUserInfo gameUserInfo,
            GameUserQuestionInfo gameUserQuestionInfo, out GameResult gameResult, out bool isRoundFinish);

        GameInfo AnalyzeByGameUserStateChanged(GameInfo gameInfo, GameUserInfo gameUserInfo, out GameResult gameResult,
            out bool isRoundFinish);

    }
}