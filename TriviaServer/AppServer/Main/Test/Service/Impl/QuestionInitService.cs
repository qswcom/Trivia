using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Module.Notification.Impl;
using Com.Qsw.Module.Notification.Interface;
using Com.Qsw.Module.Question.Impl;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.TriviaServer.AppServer.Main.Test
{
    public class QuestionInitService : IQuestionInitService
    {
        private readonly IQuestionInfoRepository questionInfoRepository;
        private readonly IQuestionInfoStatisticRepository questionInfoStatisticRepository;

        public QuestionInitService(IQuestionInfoRepository questionInfoRepository,
            IQuestionInfoStatisticRepository questionInfoStatisticRepository)
        {
            this.questionInfoRepository = questionInfoRepository;
            this.questionInfoStatisticRepository = questionInfoStatisticRepository;
        }

        public async Task Init()
        {
            (IList<QuestionInfo> questionInfos, List<QuestionInfoStatistic> questionInfoStatistics) =
                GenerateQuestions();

            foreach (QuestionInfo questionInfo in questionInfos)
            {
                await questionInfoRepository.Save(questionInfo);
            }

            foreach (QuestionInfoStatistic questionInfoStatistic in questionInfoStatistics)
            {
                await questionInfoStatisticRepository.Save(questionInfoStatistic);
            }
        }

        #region Question Generator

        private (IList<QuestionInfo> questionInfos, List<QuestionInfoStatistic> questionInfoStatistics)
            GenerateQuestions()
        {
            IList<QuestionInfo> questionInfos = new List<QuestionInfo>();
            foreach (QuestionCategory questionCategory in Enum.GetValues<QuestionCategory>())
            {
                for (int difficult = QuestionConstants.QuestionMinDifficult;
                    difficult <= QuestionConstants.QuestionMaxDifficult;
                    difficult++)
                {
                    for (var index = 0; index < 100; index++)
                    {
                        var question =
                            $"This question category is {questionCategory}, difficult {difficult} and index = {index}, Can you solve it?";
                        var wrongAnswer1 = "wrong answer1";
                        var wrongAnswer2 = "wrong answer2";
                        var rightAnswer = "right answer";
                        var questionInfo = new QuestionInfo
                        {
                            QuestionCategory = questionCategory,
                            Difficult = difficult,
                            Question = question,
                            Answers = new List<string> {wrongAnswer1, wrongAnswer2, rightAnswer},
                            CorrectAnswer = rightAnswer,
                        };
                        questionInfos.Add(questionInfo);
                    }
                }
            }

            IDictionary<(QuestionCategory questionCategory, int difficult), int> questionInfoStatisticDic =
                new Dictionary<(QuestionCategory questionCategory, int difficult), int>();
            foreach (QuestionCategory questionCategory in Enum.GetValues<QuestionCategory>())
            {
                for (int difficult = QuestionConstants.QuestionMinDifficult;
                    difficult <= QuestionConstants.QuestionMaxDifficult;
                    difficult++)
                {
                    questionInfoStatisticDic[(questionCategory, difficult)] = 0;
                }
            }

            foreach (QuestionInfo questionInfo in questionInfos)
            {
                int minDifficult = questionInfo.Difficult - QuestionConstants.QuestionSelectDifficultRange + 1;
                if (minDifficult < QuestionConstants.QuestionMinDifficult)
                {
                    minDifficult = QuestionConstants.QuestionMinDifficult;
                }

                for (int difficult = minDifficult; difficult <= questionInfo.Difficult; difficult++)
                {
                    questionInfoStatisticDic[(questionInfo.QuestionCategory, difficult)]++;
                }
            }

            var questionInfoStatistics = new List<QuestionInfoStatistic>();
            foreach (KeyValuePair<(QuestionCategory questionCategory, int difficult), int> keyValuePair in
                questionInfoStatisticDic)
            {
                var questionInfoStatistic = new QuestionInfoStatistic
                {
                    QuestionCategory = keyValuePair.Key.questionCategory,
                    Difficult = keyValuePair.Key.difficult,
                    QuestionCount = keyValuePair.Value
                };
                questionInfoStatistics.Add(questionInfoStatistic);
            }

            return (questionInfos, questionInfoStatistics);
        }

        #endregion
    }
}