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
    public class QuestionInfoService : IQuestionInfoService
    {
        private readonly ILogger logger;
        private readonly IQuestionInfoRepository questionInfoRepository;
        private readonly IQuestionInfoStatisticService questionInfoStatisticService;
        private readonly ICacheService cacheService;
        private readonly Random random;

        public QuestionInfoService(ILoggerFactory loggerFactory, IQuestionInfoRepository questionInfoRepository,
            IQuestionInfoStatisticService questionInfoStatisticService, ICacheService cacheService)
        {
            logger = loggerFactory.CreateLogger<QuestionInfoService>();
            this.questionInfoRepository = questionInfoRepository;
            this.questionInfoStatisticService = questionInfoStatisticService;
            this.cacheService = cacheService;
            random = new Random((int) DateTime.UtcNow.Ticks);
        }

        [Transaction(true)]
        public async Task<QuestionInfo> Get(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            string cacheName = GetCacheName(id);
            try
            {
                var cachedData = await cacheService.Get<QuestionInfo>(cacheName);
                if (cachedData != null)
                {
                    return cachedData;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on get cache by key {cacheName}");
            }

            QuestionInfo questionInfo = await questionInfoRepository.Get(id);
            if (questionInfo == null)
            {
                return null;
            }
            
            try
            {
                await cacheService.Set(cacheName, questionInfo);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on set cache by key {cacheName}");
            }

            return questionInfo;
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

        #region Helper

        private string GetCacheName(long id)
        {
            return $"TriviaServer_Question_{id}";
        }

        #endregion
    }
}