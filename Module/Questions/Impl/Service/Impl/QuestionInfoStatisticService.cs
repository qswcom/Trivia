using System;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Question.Interface;
using NHibernate.Linq;

namespace Com.Qsw.Module.Question.Impl
{
    public class QuestionInfoStatisticService : IQuestionInfoStatisticService
    {
        private readonly IQuestionInfoStatisticRepository questionInfoStatisticRepository;

        public QuestionInfoStatisticService(IQuestionInfoStatisticRepository questionInfoStatisticRepository)
        {
            this.questionInfoStatisticRepository = questionInfoStatisticRepository;
        }

        public Task<int> GetQuestionCount(QuestionCategory questionCategory, int minDifficult)
        {
            if (minDifficult < QuestionConstants.QuestionMinDifficult ||
                minDifficult > QuestionConstants.QuestionMaxDifficult)
            {
                throw new ArgumentOutOfRangeException(nameof(minDifficult));
            }

            int maxDifficult = minDifficult + QuestionConstants.QuestionSelectDifficultRange;
            using SessionWrapper sessionWrapper = questionInfoStatisticRepository.GetSessionWrapper(true);
            using TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction(true);
            IQueryable<QuestionInfoStatistic> questionInfoStatisticQuery =
                questionInfoStatisticRepository.GetQueryable(sessionWrapper);
            questionInfoStatisticQuery = questionInfoStatisticQuery.Where(m => m.QuestionCategory == questionCategory);
            questionInfoStatisticQuery =
                questionInfoStatisticQuery.Where(m => m.Difficult >= minDifficult && m.Difficult < maxDifficult);
            return questionInfoStatisticQuery.Select(m => m.QuestionCount).SumAsync();
        }
    }
}