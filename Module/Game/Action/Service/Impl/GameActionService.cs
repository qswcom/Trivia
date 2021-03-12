using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Game.Interface;
using Com.Qsw.Module.Question.Interface;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.Game.Action
{
    public class GameActionService : IGameActionService
    {
        private readonly IGameInfoService gameInfoService;
        private readonly IUserStateInfoService userStateInfoService;

        public GameActionService(IGameInfoService gameInfoService, IUserStateInfoService userStateInfoService)
        {
            this.gameInfoService = gameInfoService;
            this.userStateInfoService = userStateInfoService;
        }

        public async Task<GameInfo> RetrieveQuestion(long gameId, string userId)
        {
            GameInfo gameInfo = await gameInfoService.RetrieveQuestion(gameId, userId);
            gameInfo = gameInfo.Clone();
            foreach (QuestionInfo questionInfo in gameInfo.GameQuestionInfo.QuestionInfoList)
            {
                questionInfo.CorrectAnswer = null;
            }

            return gameInfo;
        }

        public async Task SubmitAnswer(long gameId, string userId, string answer)
        {
            await gameInfoService.SubmitAnswer(gameId, userId, answer);
        }

        public async Task LeftGame(long gameId, string userId)
        {
            await userStateInfoService.SetUserStateToWaiting(userId);
            await gameInfoService.LeftGame(gameId, userId);
        }
    }
}