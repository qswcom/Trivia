using System;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Question.Interface;
using NHibernate.Linq;

namespace Com.Qsw.Module.Question.Impl
{
    public class QuestionInfoService : IQuestionInfoService
    {
        private readonly IQuestionInfoRepository questionInfoRepository;
        private readonly IQuestionInfoStatisticService questionInfoStatisticService;
        private readonly Random random;

        public QuestionInfoService(IQuestionInfoRepository questionInfoRepository,
            IQuestionInfoStatisticService questionInfoStatisticService)
        {
            this.questionInfoRepository = questionInfoRepository;
            this.questionInfoStatisticService = questionInfoStatisticService;
            random = new Random((int) DateTime.UtcNow.Ticks);
        }

        [Transaction(true)]
        public async Task<QuestionInfo> RandomSelect(QuestionCategory questionCategory, int minDifficult)
        {
            if (minDifficult < QuestionConstants.QuestionMinDifficult ||
                minDifficult > QuestionConstants.QuestionMaxDifficult)
            {
                throw new ArgumentOutOfRangeException(nameof(minDifficult));
            }

            int questionCount = await questionInfoStatisticService.GetQuestionCount(questionCategory, minDifficult);
            if (questionCount <= 0)
            {
                throw new ApplicationException($"Can't find any question with {questionCategory} and {minDifficult}.");
            }

            int skipNum = random.Next(0, questionCount);
            int maxDifficult = minDifficult + QuestionConstants.QuestionSelectDifficultRange;

            using SessionWrapper sessionWrapper = questionInfoRepository.GetSessionWrapper(true);
            using TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction(true);
            IQueryable<QuestionInfo> questionInfoQuery = questionInfoRepository.GetQueryable(sessionWrapper);
            questionInfoQuery = questionInfoQuery.Where(m => m.QuestionCategory == questionCategory);
            questionInfoQuery =
                questionInfoQuery.Where(m => m.Difficult >= minDifficult && m.Difficult < maxDifficult);
            return await questionInfoQuery.Skip(skipNum).Take(1).FirstOrDefaultAsync();
        }
    }
}