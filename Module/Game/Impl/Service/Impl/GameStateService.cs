using System.Collections.Generic;
using System.Linq;
using Com.Qsw.Module.Game.Interface;

namespace Com.Qsw.Module.Game.Impl
{
    public class GameStateService : IGameStateService
    {
        public GameInfo AnalyzeByGameUserQuestionStateChanged(GameInfo gameInfo, GameUserInfo gameUserInfo,
            GameUserQuestionInfo gameUserQuestionInfo, out GameResult gameResult, out bool isRoundFinish)
        {
            gameResult = null;
            isRoundFinish = false;
            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.UnApply)
            {
                return gameInfo;
            }

            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.Process)
            {
                return gameInfo;
            }

            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.Success)
            {
                return AnalyzeByGameUserStateChanged(gameInfo, gameUserInfo, out gameResult, out isRoundFinish);
            }

            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.Failed)
            {
                gameUserInfo.GameUserState = GameUserState.Failed;
                return AnalyzeByGameUserStateChanged(gameInfo, gameUserInfo, out gameResult, out isRoundFinish);
            }

            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.Overtime)
            {
                gameUserInfo.GameUserState = GameUserState.Failed;
                return AnalyzeByGameUserStateChanged(gameInfo, gameUserInfo, out gameResult, out isRoundFinish);
            }

            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.UnApply)
            {
                gameUserInfo.GameUserState = GameUserState.Failed;
                return AnalyzeByGameUserStateChanged(gameInfo, gameUserInfo, out gameResult, out isRoundFinish);
            }

            return gameInfo;
        }


        public GameInfo AnalyzeByGameUserStateChanged(GameInfo gameInfo, GameUserInfo gameUserInfo,
            out GameResult gameResult, out bool isRoundFinish)
        {
            gameResult = null;
            isRoundFinish = false;
            (gameResult, isRoundFinish) = CheckCurrentRoundStatus(gameInfo);
            if (gameResult != null)
            {
                gameInfo.GameState = GameState.Finished;
            }

            return gameInfo;
        }

        #region Helper

        private (GameResult gameResult, bool isRoundFinish) CheckCurrentRoundStatus(GameInfo gameInfo)
        {
            int unFinishedCount = gameInfo.GameUserInfoByUserIdDictionary
                .Select(m => m.Value.GameUserQuestionInfoList.Last()).Count(m =>
                    m.GameUserQuestionState == GameUserQuestionState.NotStart ||
                    m.GameUserQuestionState == GameUserQuestionState.Process);
            if (unFinishedCount != 0)
            {
                return (null, false);
            }

            IList<string> successUserIds = gameInfo.GameUserInfoByUserIdDictionary
                .Where(m => m.Value.GameUserQuestionInfoList.Last().GameUserQuestionState ==
                            GameUserQuestionState.Success).Select(m => m.Key).ToList();
            if (successUserIds.Count == 0)
            {
                return (new GameResult {WinnerUserId = null}, false);
            }

            if (successUserIds.Count == 1)
            {
                return (new GameResult {WinnerUserId = successUserIds[0]}, false);
            }

            return (null, true);
        }

        #endregion
    }
}