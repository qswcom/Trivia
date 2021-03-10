using System;
using System.Threading.Tasks;
using Com.Qsw.Framework.Cache.Interface;
using Com.Qsw.Module.Question.Interface;
using Microsoft.Extensions.Logging;

namespace Com.Qsw.Module.Question.Impl
{
    public class QuestionInfoStatisticServiceCacheDecorator : IQuestionInfoStatisticService
    {
        private readonly IQuestionInfoStatisticService decoratedService;
        private readonly ILogger logger;
        private readonly ICacheService cacheService;

        public QuestionInfoStatisticServiceCacheDecorator(IQuestionInfoStatisticService decoratedService,
            ILoggerFactory loggerFactory, ICacheService cacheService)
        {
            this.decoratedService = decoratedService;
            logger = loggerFactory.CreateLogger<QuestionInfoStatisticServiceCacheDecorator>();
            this.cacheService = cacheService;
        }

        public async Task<int> GetQuestionCount(QuestionCategory questionCategory, int minDifficult)
        {
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

            int questionCount = await decoratedService.GetQuestionCount(questionCategory, minDifficult);
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