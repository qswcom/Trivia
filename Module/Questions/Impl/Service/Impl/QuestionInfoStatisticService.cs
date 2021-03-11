using System;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.Cache.Interface;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Question.Interface;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;

namespace Com.Qsw.Module.Question.Impl
{
    public class QuestionInfoStatisticService : IQuestionInfoStatisticService
    {
        private readonly IQuestionInfoStatisticRepository questionInfoStatisticRepository;
        private readonly ILogger logger;
        private readonly ICacheService cacheService;

        public QuestionInfoStatisticService(IQuestionInfoStatisticRepository questionInfoStatisticRepository,
            ILoggerFactory loggerFactory, ICacheService cacheService)
        {
            this.questionInfoStatisticRepository = questionInfoStatisticRepository;
            logger = loggerFactory.CreateLogger<QuestionInfoStatisticService>();
            this.cacheService = cacheService;
        }

        [Transaction(true)]
        public async Task<int> GetQuestionCount(QuestionCategory questionCategory, int minDifficult)
        {
            if (minDifficult < QuestionConstants.QuestionMinDifficult ||
                minDifficult > QuestionConstants.QuestionMaxDifficult)
            {
                throw new ArgumentOutOfRangeException(nameof(minDifficult));
            }

            string cacheName = GetCacheName(questionCategory, minDifficult);
            try
            {
                var cachedData = await cacheService.Get<CachedData>(cacheName);
                if (cachedData != null)
                {
                    return cachedData.Count;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on get cache by key {cacheName}");
            }

            int maxDifficult = minDifficult + QuestionConstants.QuestionSelectDifficultRange;
            using SessionWrapper sessionWrapper = questionInfoStatisticRepository.GetSessionWrapper(true);
            using TransactionWrapper transactionWrapper = sessionWrapper.BuildTransaction(true);
            IQueryable<QuestionInfoStatistic> questionInfoStatisticQuery =
                questionInfoStatisticRepository.GetQueryable(sessionWrapper);
            questionInfoStatisticQuery = questionInfoStatisticQuery.Where(m => m.QuestionCategory == questionCategory);
            questionInfoStatisticQuery =
                questionInfoStatisticQuery.Where(m => m.Difficult >= minDifficult && m.Difficult < maxDifficult);
            int questionCount = await questionInfoStatisticQuery.Select(m => m.QuestionCount).SumAsync();

            try
            {
                await cacheService.Set(cacheName, new CachedData {Count = questionCount});
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on set cache by key {cacheName}");
            }

            return questionCount;
        }

        #region Helper

        private string GetCacheName(QuestionCategory questionCategory, int minDifficult)
        {
            return $"TriviaServer_QuestionInfoStatisticQuestionCount_{questionCategory}_{minDifficult}";
        }

        #endregion


        #region Define

        [Serializable]
        private class CachedData
        {
            public int Count { get; set; }
        }

        #endregion
    }
}