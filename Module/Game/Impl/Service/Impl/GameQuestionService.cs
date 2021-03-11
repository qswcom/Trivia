using System;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Module.Game.Interface;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Game.Impl
{
    public class GameQuestionService : IGameQuestionService
    {
        private readonly IQuestionInfoService questionInfoService;
        private readonly Random random;

        public GameQuestionService(IQuestionInfoService questionInfoService)
        {
            this.questionInfoService = questionInfoService;
            random = new Random((int) DateTime.UtcNow.Ticks);
        }

        public async Task<QuestionInfo> GetNextQuestionInfo(GameInfo gameInfo)
        {
            QuestionCategory questionCategory = GetQuestionCategory();
            int difficult = await GetDifficult(gameInfo);
            while (true)
            {
                QuestionInfo questionInfo = await questionInfoService.RandomSelect(questionCategory, difficult);
                if (gameInfo.GameQuestionInfo.QuestionInfoList.Select(m=>m.Id).Contains(questionInfo.Id))
                {
                }
                else
                {
                    return questionInfo;
                }
            }
        }

        public Task<bool> IsRightAnswer(GameInfo gameInfo, string answer)
        {
            return Task.FromResult(gameInfo.GameQuestionInfo.QuestionInfoList.Last().CorrectAnswer == answer);
        }

        #region Helper

        private QuestionCategory GetQuestionCategory()
        {
            QuestionCategory[] questionCategories = Enum.GetValues<QuestionCategory>();
            QuestionCategory questionCategory = questionCategories[random.Next(0, questionCategories.Length)];
            return questionCategory;
        }

        private async Task<int> GetDifficult(GameInfo gameInfo)
        {
            await Task.CompletedTask;
            GameQuestionInfo gameQuestionInfo = gameInfo.GameQuestionInfo;
            if (gameQuestionInfo.CurrentCount == 0)
            {
                return QuestionConstants.QuestionMinDifficult;
            }

            QuestionInfo questionInfo = gameQuestionInfo.QuestionInfoList.Last();
            int difficult = questionInfo.Difficult + 1;
            if (difficult > QuestionConstants.QuestionMaxDifficult)
            {
                return QuestionConstants.QuestionMaxDifficult;
            }

            return difficult;
        }

        #endregion
    }
}